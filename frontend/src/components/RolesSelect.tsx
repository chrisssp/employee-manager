"use client"

import React, { useState, useRef, useEffect } from 'react'
import { Role } from '@/lib/api'
import { CheckIcon, ChevronDownIcon } from 'lucide-react'

export default function RolesSelect({ roles, value, onChange }: { roles: Role[]; value: number[]; onChange: (v: number[]) => void }) {
    const [open, setOpen] = useState(false)
    const ref = useRef<HTMLDivElement | null>(null)

    useEffect(() => {
        function onDoc(e: MouseEvent) {
            if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false)
        }
        document.addEventListener('click', onDoc)
        return () => document.removeEventListener('click', onDoc)
    }, [])

    const toggle = (id: number) => {
        if (value.includes(id)) onChange(value.filter(x => x !== id))
        else onChange([...value, id])
    }

    return (
        <div ref={ref} className="relative">
            <button type="button" onClick={() => setOpen(s => !s)} className="w-full text-left border rounded px-3 py-2 flex items-center justify-between">
                <div className="truncate">
                    {value.length === 0 ? 'Selecciona roles...' : roles.filter(r => value.includes(r.id)).map(r => r.nombre).join(', ')}
                </div>
                <ChevronDownIcon className="ml-2 w-4 h-4" />
            </button>

            {open && (
                <div className="absolute z-50 mt-2 w-full rounded border bg-white shadow p-2 max-h-52 overflow-auto">
                    {roles.map(r => (
                        <label key={r.id} className="flex items-center gap-2 px-2 py-1 hover:bg-slate-50 rounded cursor-pointer">
                            <input type="checkbox" checked={value.includes(r.id)} onChange={() => toggle(r.id)} />
                            <span className="flex-1">{r.nombre}</span>
                            {value.includes(r.id) && <CheckIcon className="w-4 h-4 text-green-600" />}
                        </label>
                    ))}
                </div>
            )}
        </div>
    )
}
