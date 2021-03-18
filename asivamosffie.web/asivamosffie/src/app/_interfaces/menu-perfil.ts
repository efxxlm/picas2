export interface MenuPerfil {
    menuPerfilId?: number,
    menuId?: number,
    perfilId?: number,
    fechaCreacion?: Date,
    activo?: boolean,
    tienePermisoCrear: boolean,
    tienePermisoLeer: boolean,
    tienePermisoEditar: boolean,
    tienePermisoEliminar: boolean,
}
