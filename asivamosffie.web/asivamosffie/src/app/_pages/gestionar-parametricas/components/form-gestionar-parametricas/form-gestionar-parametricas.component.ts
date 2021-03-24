import { Dominio } from 'src/app/core/_services/common/common.service';
import { GestionarParametricasService } from './../../../../core/_services/gestionarParametricas/gestionar-parametricas.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-gestionar-parametricas',
  templateUrl: './form-gestionar-parametricas.component.html',
  styleUrls: ['./form-gestionar-parametricas.component.scss']
})
export class FormGestionarParametricasComponent implements OnInit {

    formParametricas: FormGroup;
    tipoDominioId: number;
    dominio: any;

    get parametricas() {
        return this.formParametricas.get( 'parametricas' ) as FormArray;
    }

    constructor(
        private activatedRoute: ActivatedRoute,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private routes: Router,
        private gestionarParametricaSvc: GestionarParametricasService )
    {
        this.formParametricas = this.crearFormulario();
        this.tipoDominioId = Number( this.activatedRoute.snapshot.params.id );

        this.gestionarParametricaSvc.dominioByIdDominio( this.tipoDominioId )
            .subscribe( dominioByIdDominio => this.dominio = dominioByIdDominio[ dominioByIdDominio.length - 1 ] );
    }

    ngOnInit(): void {
    }

    crearFormulario() {
        return this.fb.group(
            {
                parametricas: this.fb.array( [
                    this.fb.group(
                        {
                            nombre: [ null, Validators.required ]
                        }
                    )
                ] )
            }
        );
    }

    addParametrica() {
        this.parametricas.push(
            this.fb.group(
                {
                    nombre: [ null, Validators.required ]
                }
            )
        );
    }

    deleteParametrica( index: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        this.parametricas.removeAt( index );
                    }
                }
            );
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    openDialogTrueFalse(modalTitle: string, modalText: string) {

        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText, siNoBoton: true }
        });

        return dialogRef.afterClosed();
    }

    guardar() {
        const pTipoDominio = { 
            tipoDominioId: this.tipoDominioId,
            dominio: []
        };

        this.parametricas.controls.forEach( control => pTipoDominio.dominio.push( { tipoDominioId: this.tipoDominioId, nombre: control.get( 'nombre' ).value } ) );

        this.gestionarParametricaSvc.createDominio( pTipoDominio )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionParametricas' ] ) );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
