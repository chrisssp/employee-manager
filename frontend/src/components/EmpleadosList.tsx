"use client"

import { useEffect, useState } from 'react'
import { empleadoService } from '@/services/empleado.service'
import { EmpleadoDTO } from '@/lib/api'
import DataTable from './DataTable'
import EntityLayout from './EntityLayout'
import { Button } from '@/components/ui/button'
import { EmpleadoForm } from '@/components/EmpleadoForm'
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription } from '@/components/ui/dialog'
import { toast } from 'sonner'
import { AlertDialog, AlertDialogContent, AlertDialogTitle, AlertDialogDescription, AlertDialogAction, AlertDialogCancel } from '@/components/ui/alert-dialog'

export default function EmpleadosList() {
    const [empleados, setEmpleados] = useState<EmpleadoDTO[]>([])
    const [loading, setLoading] = useState(true)
    const [openModal, setOpenModal] = useState(false)
    const [empleadoSeleccionado, setEmpleadoSeleccionado] = useState<EmpleadoDTO | undefined>()
    const [confirm, setConfirm] = useState<{ open: boolean; id?: number; action?: 'baja' | 'reactivar'; message?: string }>({ open: false })
    const [page, setPage] = useState<number>(1)
    const pageSize = 10

    const cargarEmpleados = async (p = 1) => {
        try {
            setLoading(true)
            const data = await empleadoService.obtenerTodos(p, pageSize)
            setEmpleados(data)
        } catch (e) {
            console.error(e)
            toast.error('Error al cargar empleados')
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => { cargarEmpleados(page) }, [page])

    const handleNew = () => { setEmpleadoSeleccionado(undefined); setOpenModal(true) }
    const handleEdit = (e: EmpleadoDTO) => { setEmpleadoSeleccionado(e); setOpenModal(true) }

    const handleBaja = (id: number) => setConfirm({ open: true, id, action: 'baja', message: '¿Confirmas dar de baja a este empleado?' })
    const handleReactivar = (id: number) => setConfirm({ open: true, id, action: 'reactivar', message: '¿Confirmas reactivar a este empleado?' })

    const performConfirm = async () => {
        if (!confirm.id || !confirm.action) return
        try {
            if (confirm.action === 'baja') {
                await empleadoService.eliminar(confirm.id)
                toast.success('Empleado dado de baja')
            } else {
                await empleadoService.reactivar(confirm.id)
                toast.success('Empleado reactivado')
            }
            await cargarEmpleados()
        } catch (e: any) {
            console.error(e)
            toast.error(e?.message || 'Error')
        } finally {
            setConfirm({ open: false })
        }
    }

    const displayed = empleados

    return (
        <EntityLayout title="Gestión de empleados" onNew={handleNew}>
            <DataTable
                columns={[
                    { key: 'nombre', header: 'Nombre completo', render: (row: EmpleadoDTO) => `${row.nombre} ${row.apellidoPaterno} ${row.apellidoMaterno || ''}` },
                    { key: 'correo', header: 'Correo', render: (row: EmpleadoDTO) => row.correo },
                    { key: 'roles', header: 'Roles', render: (row: EmpleadoDTO) => row.roles.join(', ') },
                    { key: 'estado', header: 'Estado', render: (row: EmpleadoDTO) => row.activo ? 'Activo' : 'Inactivo' },
                    {
                        key: 'acciones', header: 'Acciones', render: (row: EmpleadoDTO) => (
                            <div className="text-right flex justify-end gap-2">
                                <Button variant="outline" size="sm" onClick={() => handleEdit(row)}>Editar</Button>
                                {row.activo ? (
                                    <Button variant="destructive" size="sm" onClick={() => handleBaja(row.id)}>Baja</Button>
                                ) : (
                                    <Button variant="secondary" size="sm" onClick={() => handleReactivar(row.id)}>Reactivar</Button>
                                )}
                            </div>
                        )
                    },
                ]}
                data={displayed}
                page={page}
                pageSize={pageSize}
                onPageChange={(p) => setPage(p)}
            />

            <Dialog open={openModal} onOpenChange={(v) => { if (!v) { setOpenModal(false); cargarEmpleados() } }}>
                <DialogContent className="sm:max-w-[500px]">
                    <DialogHeader>
                        <DialogTitle>{empleadoSeleccionado ? 'Editar empleado' : 'Crear nuevo empleado'}</DialogTitle>
                        <DialogDescription>{empleadoSeleccionado ? 'Modifica los datos del empleado.' : 'Completa el formulario para registrar un nuevo empleado en el sistema.'}</DialogDescription>
                    </DialogHeader>
                    <EmpleadoForm empleado={empleadoSeleccionado} onSuccess={() => { setOpenModal(false); cargarEmpleados() }} />
                </DialogContent>
            </Dialog>

            <AlertDialog open={confirm.open} onOpenChange={(open) => setConfirm(s => ({ ...s, open }))}>
                <AlertDialogContent>
                    <AlertDialogTitle>Confirmación</AlertDialogTitle>
                    <AlertDialogDescription>{confirm.message}</AlertDialogDescription>
                    <div className="mt-4 flex gap-2 justify-end">
                        <AlertDialogCancel>Cancelar</AlertDialogCancel>
                        <AlertDialogAction onClick={performConfirm}>Confirmar</AlertDialogAction>
                    </div>
                </AlertDialogContent>
            </AlertDialog>
        </EntityLayout>
    )
}
