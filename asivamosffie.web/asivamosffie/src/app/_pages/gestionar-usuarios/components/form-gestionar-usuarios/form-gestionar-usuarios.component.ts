import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

@Component({
  selector: 'app-form-gestionar-usuarios',
  templateUrl: './form-gestionar-usuarios.component.html',
  styleUrls: ['./form-gestionar-usuarios.component.scss']
})
export class FormGestionarUsuariosComponent implements OnInit {

    esRegistroNuevo: boolean;
    formUsuario: FormGroup;
    listaProcedencia: { nombre: string, codigo: string }[] = [
        { nombre: 'FFIE/Fiduciaria', codigo: '1' },
        { nombre: 'Otro', codigo: '2' }
    ];
    listaTipoDocumento: { nombre: string, codigo: string }[] = [
        { nombre: 'Cedula de ciudadanía', codigo: '1' },
        { nombre: 'Cedula de extranjería', codigo: '2' }
    ];
    listaDepartamento: { nombre: string, codigo: string }[] = [
        { nombre: 'Boyacá', codigo: '1' }
    ];
    listaMunicipio: { nombre: string, codigo: string }[] = [
        { nombre: 'Susacón', codigo: '1' }
    ];
    editorStyle = {
        height: '50%'
    };
    config = {
        toolbar: [
            ['bold', 'italic', 'underline'],
            [{ list: 'ordered' }, { list: 'bullet' }],
            [{ indent: '-1' }, { indent: '+1' }],
            [{ align: [] }],
        ]
    };

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router )
    {
        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'crearUsuario' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'editarUsuario' ) {
                this.esRegistroNuevo = false;
                return;
            }
        } );
        this.formUsuario = this.crearFormulario();
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group(
            {
                procedencia: [ null, Validators.required ],
                primerNombre: [ null, Validators.required ],
                segundoNombre: [ null, Validators.required ],
                primerApellido: [ null, Validators.required ],
                segundoApellido: [ null, Validators.required ],
                tipoDocumento: [ null, Validators.required ],
                numeroIdentificacion: [ null, Validators.required ],
                correo: [ null, Validators.required ],
                telefonoFijo: [ null, Validators.required ],
                telefonoCelular: [ null, Validators.required ],
                departamento: [ null, Validators.required ],
                municipio: [ null, Validators.required ],
                fechaCreacion: [ null, Validators.required ],
                fechaExpiracion: [ null, Validators.required ],
                urlSoporte: [ null, Validators.required ],
                observaciones: [ null, Validators.required ],
                dependencia: [ null, Validators.required ],
                grupo: [ null, Validators.required ],
                rol: [ null, Validators.required ],
                roles: this.fb.array( [] )
            }
        );
    }

    maxLength(e: any, n: number) {
        if (e.editor.getLength() > n) {
            e.editor.deleteText(n - 1, e.editor.getLength());
        }
    }

    textoLimpio( evento: any, n: number ) {
        if ( evento !== undefined ) {
            return evento.getLength() > n ? n : evento.getLength();
        } else {
            return 0;
        }
    }

    guardar() {
        console.log( this.formUsuario );
    }

}
