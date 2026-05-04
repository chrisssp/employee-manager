"use client"

import React from 'react'
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table'

type Column<T> = {
    key: string
    header: string
    render: (row: T) => React.ReactNode
}

export default function DataTable<T>({
    columns,
    data,
    page = 1,
    pageSize = 10,
    total = 0,
    onPageChange,
}: {
    columns: Column<T>[]
    data: T[]
    page?: number
    pageSize?: number
    total?: number
    onPageChange?: (page: number) => void
}) {
    const hasTotal = !!total
    const totalPages = hasTotal ? Math.max(1, Math.ceil((total || 0) / pageSize)) : undefined

    return (
        <div className="text-sm">
            <Table>
                <TableHeader>
                    <TableRow>
                        {columns.map((col) => (
                            <TableHead key={col.key}>{col.header}</TableHead>
                        ))}
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {data.map((row, idx) => (
                        <TableRow key={idx}>
                            {columns.map((col) => (
                                <TableCell key={col.key}>{col.render(row)}</TableCell>
                            ))}
                        </TableRow>
                    ))}
                </TableBody>
            </Table>

            <div className="mt-4 flex items-center justify-between">
                <div className="text-sm text-muted-foreground">{hasTotal ? `Página ${page} de ${totalPages}` : `Página ${page}`}</div>
                <div className="flex gap-2">
                    <button
                        onClick={() => onPageChange?.(Math.max(1, page - 1))}
                        disabled={page <= 1}
                        className="px-3 py-1 rounded border disabled:opacity-50 hover:bg-slate-50"
                    >
                        ← Anterior
                    </button>
                    <button
                        onClick={() => onPageChange?.(page + 1)}
                        disabled={hasTotal ? page >= (totalPages || 1) : data.length < pageSize}
                        className="px-3 py-1 rounded border disabled:opacity-50 hover:bg-slate-50"
                    >
                        Siguiente →
                    </button>
                </div>
            </div>
        </div>
    )
}
