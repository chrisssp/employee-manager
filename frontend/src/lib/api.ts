import axios from 'axios';

// Interfaces basadas en los DTOs de tu API en C#
export interface EmpleadoDTO {
    id: number;
    nombre: string;
    apellidoPaterno: string;
    apellidoMaterno?: string;
    correo: string;
    activo: boolean;
    roles: string[];
}

export interface EmpleadoCreacionDTO {
    nombre: string;
    apellidoPaterno: string;
    apellidoMaterno?: string;
    correo: string;
    rolesIds: number[];
}

export interface Role {
    id: number;
    nombre: string;
    descripcion?: string;
}

export interface RolDTO extends Role { }

export interface RolCreacionDTO {
    nombre: string;
    descripcion: string;
}

// Interfaz para mapear tu nuevo ApiResponse<T>
export interface ApiResponse<T = any> {
    success: boolean;
    message: string;
    data: T;
    statusCode: number;
}

const api = axios.create({
    baseURL: 'http://localhost:5110/api', // Ajusta el puerto si tu backend usa otro
    headers: {
        'Content-Type': 'application/json'
    }
});

// Interceptor para extraer automáticamente los datos
api.interceptors.response.use(
    (response) => {
        // Retornar solo el data (ApiResponse<T>) extrayendo del response
        return response.data as any;
    },
    (error) => {
        if (error.response && error.response.data) {
            return Promise.reject(error.response.data as ApiResponse);
        }
        return Promise.reject(error);
    }
);

export default api;
