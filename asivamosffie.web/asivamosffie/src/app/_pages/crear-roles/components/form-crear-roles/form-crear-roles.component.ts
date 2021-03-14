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
