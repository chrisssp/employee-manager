"use client"

import { useEffect, useState } from "react"
import { logsService, LogTransacionDTO } from "@/services/logs.service"
import { toast } from 'sonner'
import EntityLayout from '@/components/EntityLayout'
import DataTable from '@/components/DataTable'
import { Button } from '@/components/ui/button'
import { Dialog, DialogContent, DialogDescription, DialogHeader, DialogTitle } from '@/components/ui/dialog'

export default function LogsList() {
    const [logs, setLogs] = useState<LogTransacionDTO[]>([])
    const [loading, setLoading] = useState<boolean>(true)
    const [page, setPage] = useState<number>(1)
    const [openPayload, setOpenPayload] = useState<boolean>(false)
    const [payloadSeleccionado, setPayloadSeleccionado] = useState<string>('')
    const pageSize = 10

    const cargarLogs = async (currentPage: number) => {
        try {
            setLoading(true)
            const data = await logsService.obtenerTodos(currentPage, pageSize)
            setLogs(data)
        } catch (error) {
            console.error("Error al cargar logs:", error)
            toast.error("Error al cargar los logs")
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => { cargarLogs(page) }, [page])

    const getVerbBadgeColor = (verb: string) => {
        switch (verb) {
            case "GET": return "bg-blue-100 text-blue-800"
            case "POST": return "bg-green-100 text-green-800"
            case "PUT": return "bg-yellow-100 text-yellow-800"
            case "DELETE": return "bg-red-100 text-red-800"
            default: return "bg-gray-100 text-gray-800"
        }
    }

    const getStatusBadgeColor = (status: number) => {
        if (status < 300) return "bg-green-100 text-green-800"
        if (status < 400) return "bg-blue-100 text-blue-800"
        if (status < 500) return "bg-yellow-100 text-yellow-800"
        return "bg-red-100 text-red-800"
    }

    const formatearFecha = (fecha: string) =>
        new Intl.DateTimeFormat('es-MX', {
            dateStyle: 'short',
            timeStyle: 'medium',
            timeZone: 'America/Mexico_City',
        }).format(new Date(fecha))

    const abrirPayload = (payload?: string) => {
        setPayloadSeleccionado(payload || '')
        setOpenPayload(true)
    }

    const formatearPayload = (payload: string) => {
        if (!payload) return 'Sin payload'

        try {
            return JSON.stringify(JSON.parse(payload), null, 2)
        } catch {
            return payload
        }
    }

    return (
        <EntityLayout title="Registro de transacciones">
            <DataTable
                columns={[
                    { key: 'fecha', header: 'Fecha', render: (l: LogTransacionDTO) => formatearFecha(l.fecha) },
                    { key: 'verbo', header: 'Verbo HTTP', render: (l: LogTransacionDTO) => <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-semibold ${getVerbBadgeColor(l.verboHttp)}`}>{l.verboHttp}</span> },
                    { key: 'endpoint', header: 'Endpoint', render: (l: LogTransacionDTO) => l.endpoint },
                    { key: 'status', header: 'Status', render: (l: LogTransacionDTO) => <span className={`inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-semibold ${getStatusBadgeColor(l.statusCode)}`}>{l.statusCode}</span> },
                    {
                        key: 'payload',
                        header: 'Payload',
                        render: (l: LogTransacionDTO) => (
                            <div className="text-right">
                                <Button
                                    type="button"
                                    variant="outline"
                                    size="sm"
                                    onClick={() => abrirPayload(l.payload)}
                                    disabled={!l.payload}
                                >
                                    Ver
                                </Button>
                            </div>
                        )
                    },
                ]}
                data={logs}
                page={page}
                pageSize={pageSize}
                onPageChange={(p) => setPage(p)}
            />

            <Dialog open={openPayload} onOpenChange={setOpenPayload}>
                <DialogContent className="sm:max-w-3xl">
                    <DialogHeader>
                        <DialogTitle>Payload de la transacción</DialogTitle>
                        <DialogDescription>
                            El contenido se muestra formateado para facilitar su lectura.
                        </DialogDescription>
                    </DialogHeader>
                    <pre className="max-h-[60vh] overflow-auto rounded-md border bg-slate-950 p-4 text-sm text-slate-50 whitespace-pre-wrap break-words">
                        {formatearPayload(payloadSeleccionado)}
                    </pre>
                </DialogContent>
            </Dialog>
        </EntityLayout>
    )
}
