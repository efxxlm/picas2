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
    listaCriteriosDePago: Dominio[] = [];
    criteriosDePagoCodigo: string;
    listaEstadoListaChequeo: Dominio[] = [];
    listaChequeoMenu: Dominio[] = [];
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
        private commonSvc: CommonService )
    {

        this.formLista = this.crearFormulario();
        // Registro nuevo === undefined
        // Editar === id del registro a editar
        console.log( this.activatedRoute.snapshot.params.id );

        this.commonSvc.listaEstadoListaChequeo()
            .subscribe( listaEstadoListaChequeo => this.listaEstadoListaChequeo = listaEstadoListaChequeo );
        this.commonSvc.listaChequeoMenu()
            .subscribe( listaChequeoMenu => {
                console.log( listaChequeoMenu );
                this.listaChequeoMenu = listaChequeoMenu;
            } );
        this.commonSvc.criteriosDePago()
            .subscribe( criteriosDePago => this.listaCriteriosDePago = criteriosDePago );

        this.activatedRoute.snapshot.url.forEach( ( urlSegment: UrlSegment ) => {
            if ( urlSegment.path === 'crearListaChequeo' ) {
                this.esRegistroNuevo = true;
            }
            if ( urlSegment.path === 'editarListaChequeo' ) {
                this.esRegistroNuevo = false;
            }
            if ( urlSegment.path === 'verDetalleListaChequeo' ) {
                this.esVerDetalle = true;
            }
        } );
    }

    ngOnInit(): void {
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
                            nombreRequisitoId: [ 0, Validators.required ],
                            nombreRequisito: [ null, Validators.required ]
                        }
                    )
                ] )
            }
        );
    }

    getRequisitoValue( requisitoCod: any ) {
        this.listaRequisitos.forEach( ( requisito, index ) => {
            if ( requisito.codigo === requisitoCod.codigo ) {
                this.listaRequisitos.splice( index, 1 );
            }
        } )
    }

    addRequisito() {
        if ( this.listaRequisitos.length > 0 ) {
            this.requisitos.push( this.fb.group(
                {
                    nombreRequisitoId: [ 0, Validators.required ],
                    nombreRequisito: [ null, Validators.required ]
                }
            ) );
        } else {
            this.openDialog( '', '<b>No se encuentran requisitos disponibles por seleccionar.</b>' );
        }
    }

    deleteRequisito( index: number ) {

        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe( value => {
                if ( value === true ) {
                    this.listaRequisitos.push( this.requisitos.controls[ index ].get( 'nombreRequisito' ).value );
                    this.requisitos.removeAt( index );
                    this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                }
            } );
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
        // this.openDialog( '', '<b>El requisito se ha creado exitosamente</b>' );
        // this.openDialog( '', '<b>El nombre de requisito ya fue utilizado, por favor verifique la información</b>' );
    }

}
