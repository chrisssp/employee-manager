"use client"

import { useEffect, useState } from "react"
import { rolesService } from "@/services/roles.service"
import { RolDTO } from "@/lib/api"
import { Button } from "@/components/ui/button"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription } from "@/components/ui/dialog"
import { RolForm } from "@/components/RolForm"
import { toast } from 'sonner'
import { AlertDialog, AlertDialogContent, AlertDialogDescription, AlertDialogTitle, AlertDialogAction, AlertDialogCancel } from '@/components/ui/alert-dialog'
import EntityLayout from '@/components/EntityLayout'
import DataTable from '@/components/DataTable'

export default function RolesList() {
    const [roles, setRoles] = useState<RolDTO[]>([])
    const [loading, setLoading] = useState<boolean>(true)
    const [openModal, setOpenModal] = useState<boolean>(false)
    const [rolSeleccionado, setRolSeleccionado] = useState<RolDTO | undefined>()
    const [confirm, setConfirm] = useState<{ open: boolean; id?: number; message?: string }>({ open: false })
    const [page, setPage] = useState<number>(1)
    const pageSize = 10

    const handleOpenCreateModal = () => { setRolSeleccionado(undefined); setOpenModal(true) }
    const handleOpenEditModal = (rol: RolDTO) => { setRolSeleccionado(rol); setOpenModal(true) }
    const handleCloseModal = () => { setOpenModal(false); setRolSeleccionado(undefined) }

    const cargarRoles = async (p = 1) => {
        try {
            setLoading(true)
            const data = await rolesService.obtenerTodos(p, pageSize)
            setRoles(data)
        } catch (error) {
            console.error("Error al cargar roles:", error)
            toast.error("Error al cargar los roles")
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => { cargarRoles(page) }, [page])

    const handleEliminar = (id: number) => { setConfirm({ open: true, id, message: '¿Estás seguro de eliminar este rol?' }) }

    const performDelete = async () => {
        if (!confirm.id) return
        try {
            await rolesService.eliminar(confirm.id)
            toast.success('Rol eliminado')
            await cargarRoles()
        } catch (error: any) {
            console.error('Error eliminating role', error)
            toast.error(error?.message || 'Error al eliminar el rol')
        } finally {
            setConfirm({ open: false })
        }
    }

    const displayed = roles

    return (
        <EntityLayout title="Gestión de roles" onNew={handleOpenCreateModal}>
            <DataTable
                columns={[
                    { key: 'nombre', header: 'Nombre', render: (r: RolDTO) => r.nombre },
                    { key: 'descripcion', header: 'Descripción', render: (r: RolDTO) => r.descripcion || '-' },
                    {
                        key: 'acciones', header: 'Acciones', render: (r: RolDTO) => (
                            <div className="text-right">
                                <Button variant="outline" size="sm" className="mr-2" onClick={() => handleOpenEditModal(r)}>Editar</Button>
                                <Button variant="destructive" size="sm" onClick={() => handleEliminar(r.id)}>Eliminar</Button>
                            </div>
                        )
                    }
                ]}
                data={displayed}
                page={page}
                pageSize={pageSize}
                onPageChange={(p) => setPage(p)}
            />

            <Dialog open={openModal} onOpenChange={handleCloseModal}>
                <DialogContent className="sm:max-w-[500px]">
                    <DialogHeader>
                        <DialogTitle>{rolSeleccionado ? "Editar rol" : "Crear nuevo rol"}</DialogTitle>
                        <DialogDescription>
                            {rolSeleccionado
                                ? "Modifica los datos del rol."
                                : "Completa el formulario para registrar un nuevo rol en el sistema."}
                        </DialogDescription>
                    </DialogHeader>
                    <RolForm rol={rolSeleccionado} onSuccess={() => { handleCloseModal(); cargarRoles() }} />
                </DialogContent>
            </Dialog>

            <AlertDialog open={confirm.open} onOpenChange={(open) => setConfirm(s => ({ ...s, open }))}>
                <AlertDialogContent>
                    <AlertDialogTitle>Confirmación</AlertDialogTitle>
                    <AlertDialogDescription>{confirm.message}</AlertDialogDescription>
                    <div className="mt-4 flex gap-2 justify-end">
                        <AlertDialogCancel>Cancelar</AlertDialogCancel>
                        <AlertDialogAction onClick={performDelete}>Confirmar</AlertDialogAction>
                    </div>
                </AlertDialogContent>
            </AlertDialog>
        </EntityLayout>
    )
}
