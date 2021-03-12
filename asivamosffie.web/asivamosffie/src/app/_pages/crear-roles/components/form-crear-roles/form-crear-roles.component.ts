import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-form-crear-roles',
  templateUrl: './form-crear-roles.component.html',
  styleUrls: ['./form-crear-roles.component.scss']
})
export class FormCrearRolesComponent implements OnInit {

    esRegistroNuevo: boolean;
    formRoles: FormGroup;
    @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    displayedColumns: string[] = [ 'fechaCreacion', 'nombreRol', 'estadoRol', 'gestion' ];
    dataSource = new MatTableDataSource();

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'nuevoRol' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'editarRol' ) {
                this.esRegistroNuevo = false;
                return;
            }
        } );
        this.formRoles = this.crearFormulario();
    }

    ngOnInit(): void {
        const dataTable = [
            {
                fechaCreacion: '24/02/2021',
                nombreRol: 'Interventor',
                estadoRol: 'Activo',
                id: 1
            }
        ]
        this.dataSource = new MatTableDataSource( dataTable );
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    }

    crearFormulario() {
        return this.fb.group(
            {
                nombreRol: [ null, Validators.required ]
            }
        );
    }

    guardar() {
        console.log( this.formRoles );
    }

}
