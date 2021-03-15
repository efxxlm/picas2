import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

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
    listaDependencia: { nombre: string, codigo: string }[] = [
        { nombre: 'Administración', codigo: '1' },
        { nombre: 'Técnica', codigo: '2' }
    ];
    listaGrupo: { nombre: string, codigo: string }[] = [
        { nombre: 'Grupo 1', codigo: '1' },
        { nombre: 'Grupo 2', codigo: '2' }
    ];
    listaRoles: { nombre: string, codigo: string }[] = [
        { nombre: 'Supervisor', codigo: '1' },
        { nombre: 'Interventor', codigo: '2' },
        { nombre: 'Apoyo a la supervisión', codigo: '3' }
    ];
    listaContratos: { nombre: string, codigo: string }[] = [
        { nombre: 'N801801', codigo: '1' },
        { nombre: 'J208208', codigo: '2' }
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

    get roles() {
        return this.formUsuario.get( 'roles' ) as FormArray;
    }

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

    async getRoles( listaRoles: any[] ) {
        const rolesArray = [ ...listaRoles ];

        if ( rolesArray.length > 0 ) {
            if ( this.roles.length > 0 ) {
                this.roles.controls.forEach( ( control, indexRol ) => {
                    const index = rolesArray.findIndex( value => value.nombre === control.get( 'nombre' ).value );
                    const rol = listaRoles.find( value => value.nombre === control.get( 'nombre' ).value );

                    if ( index  !== -1 ) {
                        rolesArray.splice( index, 1 );
                    }
                    
                    if ( rol === undefined ) {
                        this.roles.removeAt( indexRol );
                        rolesArray.splice( index, 1 );
                    }
                } );
            }

            for ( const rol of rolesArray ) {

                const listaContratos = () => {
                    return new Promise( resolve => {
                        setTimeout(() => {
                            resolve( this.listaContratos )
                        }, 500);
                    } );
                }
                const contratos = await listaContratos();

                this.roles.push( this.fb.group(
                    {
                        nombre: [ rol.nombre ],
                        contratos: [ contratos, Validators.required ],
                        contrato: [ null, Validators.required ]
                    }
                ) );
            }
        } else {
            this.roles.clear();
        }
    }

    deleteRol( index: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const listaRoles: any[] = [ ...this.formUsuario.get( 'rol' ).value ];
                        const rolIndex = listaRoles.findIndex( value => value.nombre === this.roles.controls[ index ].get( 'nombre' ).value );
                        listaRoles.splice( rolIndex, 1 );

                        this.formUsuario.get( 'rol' ).setValue( listaRoles );
                        this.roles.removeAt( index );
                    }
                }
            );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open(ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
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
