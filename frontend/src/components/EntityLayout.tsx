"use client"

import React from 'react'
import { Button } from '@/components/ui/button'

export default function EntityLayout({ title, onNew, children }: { title: string; onNew?: () => void; children: React.ReactNode }) {
    return (
        <main className="space-y-6">
            <div className="bg-white rounded-lg border p-6">
                <div className="flex justify-between items-center mb-6">
                    <h1 className="text-3xl font-bold tracking-tight">{title}</h1>
                    {onNew && <Button onClick={onNew}>+ Nuevo</Button>}
                </div>
                {children}
            </div>
        </main>
    )
}
