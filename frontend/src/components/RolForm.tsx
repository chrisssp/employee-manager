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
import { RolDTO, RolCreacionDTO } from "@/lib/api"
import { rolesService } from "@/services/roles.service"
import { toast } from 'sonner'

const formSchema = z.object({
    nombre: z.string().min(1, "El nombre es obligatorio").max(100),
    descripcion: z.string().max(500),
})

interface RolFormProps {
    onSuccess: () => void
    rol?: RolDTO
}

export function RolForm({ onSuccess, rol }: RolFormProps) {
    const isEdit = !!rol

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            nombre: rol?.nombre || "",
            descripcion: rol?.descripcion || "",
        },
    })

    async function onSubmit(values: z.infer<typeof formSchema>) {
        try {
            const payload: RolCreacionDTO = {
                nombre: values.nombre,
                descripcion: values.descripcion,
            }

            if (isEdit) {
                await rolesService.actualizar(rol!.id, payload)
            } else {
                await rolesService.crear(payload)
            }

            form.reset()
            onSuccess()
            toast.success(isEdit ? "Rol actualizado con éxito" : "Rol creado con éxito")
        } catch (error: any) {
            const actionText = isEdit ? "actualizar" : "crear"
            toast.error(`Error: ${error.message || `No se pudo ${actionText} el rol`}`)
        }
    }

    return (
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
            <Controller
                control={form.control}
                name="nombre"
                render={({ field, fieldState }) => (
                    <Field data-invalid={fieldState.invalid}>
                        <FieldLabel htmlFor={field.name}>Nombre del rol</FieldLabel>
                        <Input placeholder="Ej. Administrador" {...field} id={field.name} aria-invalid={fieldState.invalid} />
                        {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                    </Field>
                )}
            />

            <Controller
                control={form.control}
                name="descripcion"
                render={({ field, fieldState }) => (
                    <Field data-invalid={fieldState.invalid}>
                        <FieldLabel htmlFor={field.name}>Descripción</FieldLabel>
                        <textarea {...field} id={field.name} placeholder="Describe las responsabilidades del rol..." className="w-full border rounded p-2" rows={3} />
                        {fieldState.invalid && <FieldError errors={[fieldState.error]} />}
                    </Field>
                )}
            />

            <Button type="submit" className="w-full">{isEdit ? "Actualizar rol" : "Crear rol"}</Button>
        </form>
    )
}
