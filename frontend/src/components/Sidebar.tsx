"use client"

import React from 'react'
import { Users, FileText, Activity, Home, Settings } from 'lucide-react'
import Link from 'next/link'
import { usePathname } from 'next/navigation'

const items = [
    { href: '/', label: 'Inicio', icon: Home },
    { href: '/empleados', label: 'Empleados', icon: Users },
    { href: '/roles', label: 'Roles', icon: FileText },
    { href: '/logs', label: 'Logs', icon: Activity },
]

const bottom = { href: '/settings', label: 'Configuración', icon: Settings }

export default function Sidebar() {
    const pathname = usePathname() || '/'

    return (
        <aside className="w-64 border-r p-4 bg-white">
            <div className="mb-6 text-xl font-semibold">Empresa</div>
            <nav className="flex flex-col gap-2">
                {items.map((it) => {
                    const Icon = it.icon
                    const active = pathname === it.href || pathname.startsWith(it.href + '/')
                    return (
                        <Link
                            key={it.href}
                            href={it.href}
                            className={`w-full flex items-center gap-2 p-2 rounded min-h-[36px] ${active ? 'bg-slate-700 text-white' : 'hover:bg-slate-100'}`}
                        >
                            <div className="w-4 h-4 flex items-center justify-center"><Icon className="w-4 h-4" /></div>
                            <span className="truncate">{it.label}</span>
                        </Link>
                    )
                })}
            </nav>
            <div className="mt-6 pt-6 border-t">
                <Link href={bottom.href} className={`w-full flex items-center gap-2 p-2 rounded min-h-[36px] hover:bg-slate-100`}>
                    <div className="w-4 h-4 flex items-center justify-center"><Settings className="w-4 h-4" /></div>
                    <span className="truncate">{bottom.label}</span>
                </Link>
            </div>
        </aside>
    )
}
