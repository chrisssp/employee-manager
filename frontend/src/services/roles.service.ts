import api, { RolDTO, ApiResponse, RolCreacionDTO } from '@/lib/api'

export const rolesService = {
    obtenerTodos: async (page?: number, pageSize?: number): Promise<RolDTO[]> => {
        const params: any = {}
        if (page) params.page = page
        if (pageSize) params.pageSize = pageSize
        const response = await api.get('/roles', { params }) as ApiResponse<RolDTO[]>
        if (!response.success) throw new Error(response.message)
        return response.data
    },

    obtenerPorId: async (id: number): Promise<RolDTO> => {
        const response = await api.get(`/roles/${id}`) as ApiResponse<RolDTO>
        if (!response.success) throw new Error(response.message)
        return response.data
    },

    crear: async (dto: RolCreacionDTO): Promise<RolDTO> => {
        const response = await api.post('/roles', dto) as ApiResponse<RolDTO>
        if (!response.success) throw new Error(response.message)
        return response.data
    },

    actualizar: async (id: number, dto: RolCreacionDTO): Promise<RolDTO> => {
        const response = await api.put(`/roles/${id}`, dto) as ApiResponse<RolDTO>
        if (!response.success) throw new Error(response.message)
        return response.data
    },

    eliminar: async (id: number): Promise<void> => {
        const response = await api.delete(`/roles/${id}`) as ApiResponse
        if (!response.success) throw new Error(response.message)
    }
}
