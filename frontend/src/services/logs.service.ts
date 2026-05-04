import api, { ApiResponse } from '@/lib/api'

export interface LogTransacionDTO {
    id: number
    verboHttp: string
    endpoint: string
    statusCode: number
    payload: string
    fecha: string
}

export const logsService = {
    obtenerTodos: async (page: number = 1, pageSize: number = 50): Promise<LogTransacionDTO[]> => {
        const response = await api.get(`/logs?page=${page}&pageSize=${pageSize}`) as ApiResponse<LogTransacionDTO[]>
        if (!response.success) throw new Error(response.message)
        return response.data
    },

    obtenerPorId: async (id: number): Promise<LogTransacionDTO> => {
        const response = await api.get(`/logs/${id}`) as ApiResponse<LogTransacionDTO>
        if (!response.success) throw new Error(response.message)
        return response.data
    }
}
