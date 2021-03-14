import { from } from 'rxjs';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, Renderer2 } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-form-crear-roles',
  templateUrl: './form-crear-roles.component.html',
  styleUrls: ['./form-crear-roles.component.scss']
})
export class FormCrearRolesComponent implements OnInit {

    esRegistroNuevo: boolean;
    formRoles: FormGroup;
    @ViewChild( MatSort, { static: true } ) sort: MatSort;
    @ViewChild( 'matTable', { static: false, read: ElementRef } ) table: ElementRef;
    @ViewChild( 'heightAside', { static: false, read: ElementRef } ) heightAside: ElementRef;
    displayedColumns: string[] = [ 'funcionalidad', 'crear', 'modificar', 'consultar', 'eliminar' ];
    dataSource = new MatTableDataSource();

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private renderer: Renderer2 )
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
                funcionalidad: 'Gestionar usuarios',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Gestionar lista de chequeo',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Crear roles',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            },
            {
                funcionalidad: 'Gestionar parametricas',
                crear: null,
                modificar: null,
                consultar: null,
                eliminar: null
            }
        ]
        this.dataSource = new MatTableDataSource( dataTable );
        setTimeout(() => {
            this.renderer.setStyle( this.heightAside.nativeElement, 'height', `${ this.table.nativeElement.querySelector('tbody').offsetHeight }px` );
        }, 5);
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
