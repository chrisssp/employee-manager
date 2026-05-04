"use client"

import React from 'react'
import { Bell, Search, User } from 'lucide-react'

export default function TopNav() {
    return (
        <header className="w-full border-b p-3 bg-white flex items-center justify-between">
            <div className="flex items-center gap-4">
                <div className="hidden md:block">
                    <input className="border rounded px-2 py-1" placeholder="Buscar..." />
                </div>
            </div>
            <div className="flex items-center gap-3">
                <button className="p-2 rounded hover:bg-slate-100"><Bell /></button>
                <button className="p-2 rounded hover:bg-slate-100"><User /></button>
            </div>
        </header>
    )
}
