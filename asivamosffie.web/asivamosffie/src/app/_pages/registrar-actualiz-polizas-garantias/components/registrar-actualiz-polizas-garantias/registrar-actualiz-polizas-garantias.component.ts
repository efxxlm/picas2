import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { MatDialog } from '@angular/material/dialog';
import { ActualizarPolizasService } from './../../../../core/_services/actualizarPolizas/actualizar-polizas.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatAutocompleteTrigger } from '@angular/material/autocomplete';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-registrar-actualiz-polizas-garantias',
  templateUrl: './registrar-actualiz-polizas-garantias.component.html',
  styleUrls: ['./registrar-actualiz-polizas-garantias.component.scss']
})
export class RegistrarActualizPolizasGarantiasComponent implements OnInit {

    verAyuda = false;
    addressForm = this.fb.group(
        {
            numeroContrato: [ null ]
        }
    );
    listContrato: any[] = [];
    listActualizacion: any[] = [];
    estaEditando = false;

    constructor(
        private fb: FormBuilder, 
        private routes: Router,
        private dialog: MatDialog,
        private actualizarPolizaSvc: ActualizarPolizasService )
    {
        this.actualizarPolizaSvc.getListVActualizacionPolizaYGarantias()
            .subscribe(
                getListVActualizacionPolizaYGarantias => {
                    if ( getListVActualizacionPolizaYGarantias.length > 0 ) {
                        this.listActualizacion = getListVActualizacionPolizaYGarantias;
                    }
                }
            )
    }

    ngOnInit(): void {
    }

    loadDataSource() {
    }

    getContratos(  ) {
        if ( this.addressForm.get( 'numeroContrato' ).value !== null ) {
            if ( this.addressForm.get( 'numeroContrato' ).value.length > 0 ) {
                this.listContrato = [];
                this.actualizarPolizaSvc.getContratoByNumeroContrato( this.addressForm.get( 'numeroContrato' ).value )
                    .subscribe(
                        response => {
                            console.log( response );
                            this.listContrato = response;

                            if ( response.length === 0 ) {
                                this.openDialog( '', '<b>No se encontraron contratos relacionados.</b>' );
                                this.addressForm.get( 'numeroContrato' ).setValue( null )
                            }
                        }
                    )
            }
        }
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

}
