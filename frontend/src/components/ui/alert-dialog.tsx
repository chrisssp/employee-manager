"use client"

import * as React from 'react'
import * as AlertDialogPrimitive from '@radix-ui/react-alert-dialog'
import { Button } from './button'
import { cn } from '@/lib/utils'

function AlertDialog({ ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Root>) {
    return <AlertDialogPrimitive.Root {...props} />
}

function AlertDialogTrigger({ ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Trigger>) {
    return <AlertDialogPrimitive.Trigger {...props} />
}

function AlertDialogPortal({ ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Portal>) {
    return <AlertDialogPrimitive.Portal {...props} />
}

function AlertDialogOverlay({ className, ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Overlay>) {
    return <AlertDialogPrimitive.Overlay className={cn('fixed inset-0 z-50 bg-black/30', className)} {...props} />
}

function AlertDialogContent({ className, children, ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Content>) {
    return (
        <AlertDialogPortal>
            <AlertDialogOverlay />
            <AlertDialogPrimitive.Content className={cn('fixed z-50 top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 rounded bg-popover p-4 shadow', className)} {...props}>
                {children}
            </AlertDialogPrimitive.Content>
        </AlertDialogPortal>
    )
}

function AlertDialogTitle({ className, ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Title>) {
    return <AlertDialogPrimitive.Title className={cn('font-medium text-base', className)} {...props} />
}

function AlertDialogDescription({ className, ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Description>) {
    return <AlertDialogPrimitive.Description className={cn('text-sm text-muted-foreground', className)} {...props} />
}

function AlertDialogAction({ ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Action>) {
    return <AlertDialogPrimitive.Action asChild {...props}><Button>{props.children}</Button></AlertDialogPrimitive.Action>
}

function AlertDialogCancel({ ...props }: React.ComponentProps<typeof AlertDialogPrimitive.Cancel>) {
    return <AlertDialogPrimitive.Cancel asChild {...props}><Button variant="outline">{props.children}</Button></AlertDialogPrimitive.Cancel>
}

export {
    AlertDialog,
    AlertDialogTrigger,
    AlertDialogPortal,
    AlertDialogOverlay,
    AlertDialogContent,
    AlertDialogTitle,
    AlertDialogDescription,
    AlertDialogAction,
    AlertDialogCancel,
}
