"use client"

import { Controller, useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import * as z from "zod"
import { Button } from "@/components/ui/button"
import {
    Field,
    FieldError,
    FieldLabel
} from "@/components/ui/field"
import { Input } from "@/components/ui/input"
import { EmpleadoDTO, EmpleadoCreacionDTO, Role } from "@/lib/api"
import { empleadoService } from "@/services/empleado.service"
import { useEffect, useState } from 'react'
import { toast } from 'sonner'
import RolesSelect from '@/components/RolesSelect'

// Validaciones en el cliente
const formSchema = z.object({
    nombre: z.string().min(1, "El nombre es obligatorio").max(50),
    apellidoPaterno: z.string().min(1, "El apellido paterno es obligatorio").max(50),
    apellidoMaterno: z.string().max(50).optional(),
    correo: z.string().email("Formato de correo inválido").max(100),
    rolesIds: z.array(z.number()).min(1, "Debes seleccionar al menos un rol"),
})

interface EmpleadoFormProps {
    onSuccess: () => void
    empleado?: EmpleadoDTO
}

export function EmpleadoForm({ onSuccess, empleado }: EmpleadoFormProps) {
    const isEdit = !!empleado

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            nombre: empleado?.nombre || "",
            apellidoPaterno: empleado?.apellidoPaterno || "",
            apellidoMaterno: empleado?.apellidoMaterno || "",
            correo: empleado?.correo || "",
            rolesIds: [],
        },
    })

    const [roles, setRoles] = useState<Role[]>([])

    useEffect(() => {
        let mounted = true
        empleadoService.obtenerRoles().then(r => {
            if (!mounted) return
            setRoles(r)
            if (empleado) {
                const selected = r.filter(x => empleado.roles.includes(x.nombre)).map(x => x.id)
                form.setValue('rolesIds', selected)
            }
        }).catch(() => {
            // ignore - roles will be empty
        })
        return () => { mounted = false }
    }, [empleado])

    async function onSubmit(values: z.infer<typeof formSchema>) {
        try {
            const payload: EmpleadoCreacionDTO = {
                nombre: values.nombre,
                apellidoPaterno: values.apellidoPaterno,
                apellidoMaterno: values.apellidoMaterno,
                correo: values.correo,
                rolesIds: values.rolesIds,
            }

            if (isEdit) {
                await empleadoService.actualizar(empleado!.id, payload)
            } else {
                await empleadoService.crear(payload)
            }

            form.reset()
            onSuccess()
            toast.success(isEdit ? "Empleado actualizado con éxito" : "Empleado creado con éxito")
        } catch (error: any) {
            const actionText = isEdit ? "actualizar" : "crear"
            toast.error(`Error: ${error.message || `No se pudo ${actionText} el empleado`}`)
        }
    }

    return (
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
                <Controller
                    control={form.control}
                    name="nombre"
                    render={({ field, fieldState }) => (
                        <Field data-invalid={fieldState.invalid}>
                            <FieldLabel htmlFor={field.name}>Nombre</FieldLabel>
                            <Input placeholder="Ej. Carlos" {...field} id={field.name} aria-invalid={fieldState.invalid} />
                            {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                        </Field>
                    )}
                />
                <Controller
                    control={form.control}
                    name="correo"
                    render={({ field, fieldState }) => (
                        <Field data-invalid={fieldState.invalid}>
                            <FieldLabel htmlFor={field.name}>Correo electrónico</FieldLabel>
                            <Input placeholder="carlos@empresa.com.mx" {...field} id={field.name} aria-invalid={fieldState.invalid} />
                            {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                        </Field>
                    )}
                />
            </div>

            <div className="grid grid-cols-2 gap-4">
                <Controller
                    control={form.control}
                    name="apellidoPaterno"
                    render={({ field, fieldState }) => (
                        <Field data-invalid={fieldState.invalid}>
                            <FieldLabel htmlFor={field.name}>Apellido paterno</FieldLabel>
                            <Input placeholder="López" {...field} id={field.name} aria-invalid={fieldState.invalid} />
                            {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                        </Field>
                    )}
                />
                <Controller
                    control={form.control}
                    name="apellidoMaterno"
                    render={({ field, fieldState }) => (
                        <Field data-invalid={fieldState.invalid}>
                            <FieldLabel htmlFor={field.name}>Apellido materno (Opcional)</FieldLabel>
                            <Input placeholder="García" {...field} value={field.value || ""} id={field.name} aria-invalid={fieldState.invalid} />
                            {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                        </Field>
                    )}
                />
            </div>

            <div>
                <Controller
                    control={form.control}
                    name="rolesIds"
                    render={({ field, fieldState }) => (
                        <Field data-invalid={fieldState.invalid}>
                            <FieldLabel>Roles</FieldLabel>
                            <RolesSelect roles={roles} value={field.value || []} onChange={field.onChange} />
                            {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                        </Field>
                    )}
                />
            </div>

            <Button type="submit" className="w-full">{isEdit ? "Actualizar empleado" : "Registrar empleado"}</Button>
        </form>
    )
}
