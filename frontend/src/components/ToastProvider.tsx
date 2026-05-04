"use client"

import React, { createContext, useContext, useState, useCallback } from 'react'

type Toast = { id: number; message: string; type?: 'success' | 'error' }

const ToastContext = createContext<{ show: (message: string, type?: Toast['type']) => void } | undefined>(undefined)

export const useToast = () => {
    const ctx = useContext(ToastContext)
    if (!ctx) throw new Error('useToast must be used within ToastProvider')
    return ctx
}

export function ToastProvider({ children }: { children: React.ReactNode }) {
    const [toasts, setToasts] = useState<Toast[]>([])

    const show = useCallback((message: string, type: Toast['type'] = 'success') => {
        const id = Date.now()
        setToasts((t) => [...t, { id, message, type }])
        setTimeout(() => setToasts((t) => t.filter(x => x.id !== id)), 3500)
    }, [])

    return (
        <ToastContext.Provider value={{ show }}>
            {children}
            <div className="fixed right-4 bottom-6 flex flex-col gap-2 z-[9999]">
                {toasts.map(t => (
                    <div key={t.id} className={`px-4 py-2 rounded shadow text-white ${t.type === 'error' ? 'bg-red-600' : 'bg-green-600'}`}>
                        {t.message}
                    </div>
                ))}
            </div>
        </ToastContext.Provider>
    )
}
