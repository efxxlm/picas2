import { GestionarListaChequeoService } from './../../../../core/_services/gestionarListaChequeo/gestionar-lista-chequeo.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-crear-banco',
  templateUrl: './form-crear-banco.component.html',
  styleUrls: ['./form-crear-banco.component.scss']
})
export class FormCrearBancoComponent implements OnInit {

    formBanco: FormGroup;
    esRegistroNuevo: boolean;
    listaChequeoItemId = 0;
    booleanosEstadoReq: any[] = [
        { viewValue: 'Activo', value: true },
        { viewValue: 'Inactivo', value: false }
    ];

    constructor(
        private activatedRoute: ActivatedRoute,
        private routes: Router,
        private fb: FormBuilder,
        private dialog: MatDialog,
        private listaChequeoSvc: GestionarListaChequeoService )
    {
        // Registro nuevo === undefined
        // Editar === id del registro a editar
        this.formBanco = this.crearFormulario();
        if ( activatedRoute.snapshot.params.id !== undefined ) {
            this.esRegistroNuevo = false;
        } else {
            this.esRegistroNuevo = true;
        }
    }

    ngOnInit(): void {
    }

    crearFormulario(): FormGroup {
        return this.fb.group(
            {
                nombreRequisito: [ null, Validators.required ],
                estadoRequisito: [ null, Validators.required ]
            }
        );
    }

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        console.log( this.formBanco );
        // this.openDialog( '', '<b>El requisito se ha creado exitosamente</b>' );
        // this.openDialog( '', '<b>El nombre de requisito ya fue utilizado, por favor verifique la información</b>' );

        const pListaChequeoItem = {
            listaChequeoItemId: this.listaChequeoItemId,
            nombre: this.formBanco.get( 'nombreRequisito' ).value,
            activo: this.formBanco.get( 'estadoRequisito' ).value
        };

        this.listaChequeoSvc.createEditItem( pListaChequeoItem )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', { skipLocationChange: true } ).then( () => this.routes.navigate( [ '/gestionListaChequeo' ] ) );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
