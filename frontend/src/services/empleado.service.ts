import api, { EmpleadoDTO, ApiResponse, EmpleadoCreacionDTO, Role } from '@/lib/api';

export const empleadoService = {
    obtenerTodos: async (page?: number, pageSize?: number): Promise<EmpleadoDTO[]> => {
        const params: any = {}
        if (page) params.page = page
        if (pageSize) params.pageSize = pageSize
        const response = await api.get('/empleados', { params }) as ApiResponse<EmpleadoDTO[]>;
        if (!response.success) throw new Error(response.message);
        return response.data;
    },

    crear: async (dto: EmpleadoCreacionDTO): Promise<EmpleadoDTO> => {
        const response = await api.post('/empleados', dto) as ApiResponse<EmpleadoDTO>;
        if (!response.success) throw new Error(response.message);
        return response.data;
    },

    actualizar: async (id: number, dto: EmpleadoCreacionDTO): Promise<EmpleadoDTO> => {
        const response = await api.put(`/empleados/${id}`, dto) as ApiResponse<EmpleadoDTO>;
        if (!response.success) throw new Error(response.message);
        return response.data;
    },

    eliminar: async (id: number): Promise<void> => {
        const response = await api.delete(`/empleados/${id}`) as ApiResponse;
        if (!response.success) throw new Error(response.message);
    }
    ,
    reactivar: async (id: number): Promise<void> => {
        const response = await api.post(`/empleados/${id}/reactivar`) as ApiResponse;
        if (!response.success) throw new Error(response.message);
    }
    ,
    obtenerRoles: async (): Promise<Role[]> => {
        const response = await api.get('/roles') as ApiResponse<Role[]>;
        if (!response.success) throw new Error(response.message);
        return response.data;
    }
};
