import { delay } from 'rxjs/operators';
import { GestionarListaChequeoService } from './../../../../core/_services/gestionarListaChequeo/gestionar-lista-chequeo.service';
import { Dominio } from './../../../../core/_services/common/common.service';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-lista-chequeo',
  templateUrl: './form-lista-chequeo.component.html',
  styleUrls: ['./form-lista-chequeo.component.scss']
})
export class FormListaChequeoComponent implements OnInit {

    formLista: FormGroup;
    esRegistroNuevo: boolean;
    esVerDetalle = false;
    listaChequeoId = 0;
    listaCriteriosDePago: Dominio[] = [];
    criteriosDePagoCodigo: string;
    listaEstadoListaChequeo: Dominio[] = [];
    listaChequeoMenu: Dominio[] = [];
    listaItems: { nombre: string, listaChequeoItemId: number }[] = [];
    listaCriterio = [
        { codigo: '1', nombre: 'Estudios y diseños interventoría hasta el 90%' },
        { codigo: '2', nombre: 'Precontrucción-Obra' }
    ];
    listaRequisitos = [
        { codigo: '1', nombre: 'NIT actualizado del contratista a cargo de la obra' },
        { codigo: '2', nombre: 'Requisito 0001' }
    ];

    get requisitos() {
        return this.formLista.get( 'requisitos' ) as FormArray;
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private routes: Router,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private listaChequeoSvc: GestionarListaChequeoService,
        private commonSvc: CommonService )
    {

        this.formLista = this.crearFormulario();
        // GET estado del campo "Estado de la lista"
        this.commonSvc.listaEstadoListaChequeo()
            .subscribe( listaEstadoListaChequeo => {
                this.listaEstadoListaChequeo = listaEstadoListaChequeo;
                // GET lista desplegable del campo "La lista de chequeo está asociada a"
                this.commonSvc.listaChequeoMenu()
                    .subscribe( listaChequeoMenu => {
                        this.listaChequeoMenu = listaChequeoMenu;
                        const criterioPagoDominio = listaChequeoMenu.find( menu => menu.nombre === 'Criterios de pago' );

                        if ( criterioPagoDominio !== undefined ) {
                            this.criteriosDePagoCodigo = criterioPagoDominio.codigo;
                        }
                        // GET lista desplegable del campo "Criterio de pago"
                        this.commonSvc.criteriosDePago()
                            .subscribe( criteriosDePago => {
                                this.listaCriteriosDePago = criteriosDePago;
                                // GET lista desplegable del campo "Nombre del requisito"
                                this.listaChequeoSvc.getListItem()
                                    .subscribe(
                                        items => {
                                            items.forEach( item => {
                                                if ( item.activo === true ) {
                                                    this.listaItems.push( { nombre: item.nombre, listaChequeoItemId: item.listaChequeoItemId } );
                                                }
                                            } );
                                            // Registro nuevo === undefined
                                            // Editar === id del registro a editar
                                            if ( this.activatedRoute.snapshot.params.id !== undefined ) {
                                                // GET data de la lista de chequeo
                                                this.listaChequeoSvc.getListaChequeoItemByListaChequeoId( this.activatedRoute.snapshot.params.id )
                                                    .subscribe(
                                                        listaChequeo => {
                                                            this.formLista.get( 'estadoLista' ).setValue( listaChequeo.estadoCodigo !== undefined ? this.listaEstadoListaChequeo.find( estado => estado.codigo === listaChequeo.estadoCodigo ).codigo : null );
                                                            this.formLista.get( 'nombreLista' ).setValue( listaChequeo.nombre !== undefined ? listaChequeo.nombre : null );
                                                            this.formLista.get( 'tipoLista' ).setValue( listaChequeo.estadoMenuCodigo !== undefined ? this.listaChequeoMenu.find( estado => estado.codigo === listaChequeo.estadoMenuCodigo ).codigo : null );
                                                            this.formLista.get( 'criterioPago' ).setValue( listaChequeo.criterioPagoCodigo !== undefined ? this.listaCriteriosDePago.find( estado => estado.codigo === listaChequeo.criterioPagoCodigo ).codigo : null );
                                                            
                                                            if ( listaChequeo.listaChequeoListaChequeoItem.length > 0 ) {
                                                                this.requisitos.clear();

                                                                listaChequeo.listaChequeoListaChequeoItem.forEach( item => {

                                                                    const requisito = this.listaItems.find( itemValue => itemValue.listaChequeoItemId === item.listaChequeoItemId );

                                                                    if ( requisito !== undefined ) {
                                                                        this.requisitos.push(
                                                                            this.fb.group(
                                                                                {
                                                                                    listaChequeoListaChequeoItemId: [ item.listaChequeoListaChequeoItemId, Validators.required ],
                                                                                    nombreRequisito: [ requisito, Validators.required ]
                                                                                }
                                                                            )
                                                                        );

                                                                        const listaItemsIndex = this.listaItems.findIndex( itemValue => itemValue === requisito );
                                                                        this.listaItems.splice( listaItemsIndex, 1 );
                                                                    }
                                                                } );
                                                            }
                                                            console.log( listaChequeo );
                                                        }
                                                    );
                                            }
                                        }
                                    );
                            } );
                    } );
            } );

        this.formLista.get( 'nombreLista' ).valueChanges
            .pipe(
                delay( 2000 )
            )
            .subscribe( nombre => {
                if ( this.formLista.get( 'nombreLista' ).dirty === true && this.formLista.get( 'nombreLista' ).value !== null ) {
                    const pListaChequeo = { nombre };
                    this.listaChequeoSvc.getValidateExistNameCheckList( pListaChequeo )
                        .subscribe(
                            response => {
                                if ( response === true && this.formLista.get( 'nombreLista' ).value === nombre ) {
                                    this.openDialog( '', '<b>El nombre de la lista de chequeo ya fue utilizado, por favor verifique la información.</b>' );
                                    this.formLista.get( 'nombreLista' ).setValue( null );
                                }
                            }
                        );
                }
            } );

        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'crearListaChequeo' ) {
                this.esRegistroNuevo = true;
                return;
            }
            if ( urlSegment.path === 'editarListaChequeo' ) {
                this.esRegistroNuevo = false;
                return;
            }
            if ( urlSegment.path === 'verDetalleListaChequeo' ) {
                this.esVerDetalle = true;
                return;
            }
        } );
    }

    ngOnInit(): void {
    }

    getEstado( codigo: string ) {
        if ( this.listaEstadoListaChequeo.length > 0 ) {
            const listaEstado = this.listaEstadoListaChequeo.find( estado => estado.codigo === codigo );

            if ( listaEstado !== undefined ) {
                return listaEstado.nombre;
            }
        }
    }

    getTipoLista( codigo: string ) {
        if ( this.listaChequeoMenu.length > 0 ) {
            const tipoLista = this.listaChequeoMenu.find( tipo => tipo.codigo === codigo );

            if ( tipoLista !== undefined ) {
                return tipoLista.nombre;
            }
        }
    }

    getTipoCriterio( codigo: string ) {
        if ( this.listaCriteriosDePago.length > 0 && codigo !== null ) {
            const criterio = this.listaCriteriosDePago.find( criterio => criterio.codigo === codigo );

            if ( criterio !== undefined ) {
                return criterio.nombre;
            }
        }
    }

    crearFormulario(): FormGroup {
        return this.fb.group(
            {
                estadoLista: [ null, Validators.required ],
                nombreLista: [ null, Validators.required ],
                tipoLista: [ null, Validators.required ],
                criterioPago: [ null, Validators.required ],
                requisitos: this.fb.array( [
                    this.fb.group(
                        {
                            listaChequeoListaChequeoItemId: [ 0, Validators.required ],
                            nombreRequisito: [ null, Validators.required ]
                        }
                    )
                ] )
            }
        );
    }

    getRequisitoValue( item: { nombre: string, listaChequeoItemId: number } ) {
        this.listaItems.forEach( ( itemValue, index ) => {
            if ( itemValue.listaChequeoItemId === item.listaChequeoItemId ) {
                this.listaItems.splice( index, 1 );
            }
        } );
    }

    addRequisito() {
        if ( this.listaRequisitos.length > 0 ) {
            this.requisitos.push( this.fb.group(
                {
                    listaChequeoListaChequeoItemId: [ 0, Validators.required ],
                    nombreRequisito: [ null, Validators.required ]
                }
            ) );
        } else {
            this.openDialog( '', '<b>No se encuentran requisitos disponibles por seleccionar.</b>' );
        }
    }

    deleteRequisito( index: number ) {

        if ( this.requisitos.controls[ index ].get( 'listaChequeoListaChequeoItemId' ).value === 0 ) {
            this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
                .subscribe( value => {
                    if ( value === true ) {
                        this.listaItems.push( this.requisitos.controls[ index ].get( 'nombreRequisito' ).value );
                        this.requisitos.removeAt( index );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                } );
        } else {
            this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
                .subscribe( value => {
                    if ( value === true ) {
                        this.listaItems.push( this.requisitos.controls[ index ].get( 'nombreRequisito' ).value );
                        this.requisitos.removeAt( index );
                        this.listaChequeoSvc.deleteListaChequeoItem( this.requisitos.controls[ index ].get( 'listaChequeoListaChequeoItemId' ).value )
                            .subscribe(
                                response => {
                                    this.openDialog( '', `<b>${ response.message }</b>` );
                                },
                                err => this.openDialog( '', `<b>${ err.message }</b>` )
                            );
                    }
                } );
        }

    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '30em',
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

    guardar() {
        console.log( this.formLista );

        const getListaChequeoItem = () => {
            const listaChequeo = [];
            this.requisitos.controls.forEach( ( control, index ) => {
                listaChequeo.push(
                    {
                        orden: index + 1,
                        listaChequeoId: this.listaChequeoId,
                        listaChequeoItemId: control.get( 'nombreRequisito' ).value.listaChequeoItemId,
                        listaChequeoListaChequeoItemId: control.get( 'listaChequeoListaChequeoItemId' ).value
                    }
                );
            } );

            return listaChequeo;
        };

        const pListaChequeo = {
            listaChequeoId: this.listaChequeoId,
            estadoCodigo: this.formLista.get( 'estadoLista' ).value,
            nombre: this.formLista.get( 'nombreLista' ).value,
            estadoMenuCodigo: this.formLista.get( 'tipoLista' ).value,
            criterioPagoCodigo: this.formLista.get( 'criterioPago' ).value,
            listaChequeoListaChequeoItem: getListaChequeoItem()
        }

        this.listaChequeoSvc.createEditCheckList( pListaChequeo )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionListaChequeo' ] ) );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
