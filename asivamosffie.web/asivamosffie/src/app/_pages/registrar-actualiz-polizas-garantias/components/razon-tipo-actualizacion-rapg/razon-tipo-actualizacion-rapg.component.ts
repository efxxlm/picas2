import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-razon-tipo-actualizacion-rapg',
  templateUrl: './razon-tipo-actualizacion-rapg.component.html',
  styleUrls: ['./razon-tipo-actualizacion-rapg.component.scss']
})
export class RazonTipoActualizacionRapgComponent implements OnInit {

    addressForm = this.fb.group({
        contratoPolizaActualizacionId: [ 0 ],
        razonActualizacion: [null, Validators.required],
        fechaExpedicion: [null, Validators.required],
        polizasYSeguros: [null, Validators.required],
        seguros: this.fb.array( [] )
    });
    razonActualizacionArray : any[] = [
      {
        valor:'1',
        nombre: 'Terminación de contrato'
      }
    ]
    polizasYSegurosArray: Dominio[] = [];
    tipoActualizacionArray: any[] = [{
      codigo: '1',
      nombre: 'Fecha'
    },
    {
      codigo: '2',
      nombre: 'Valor'
    }];
    estaEditando = false;

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private common: CommonService,
        private fb: FormBuilder,
        private router: Router,
        private dialog: MatDialog )
    {
        this.common.listaGarantiasPolizas()
            .subscribe( listaGarantiasPolizas => this.polizasYSegurosArray = listaGarantiasPolizas );
    }

    ngOnInit(): void {
    }
  
    getvalues( codigoSeguro: string ) {
        const listaSeguros = [ ...codigoSeguro ];

        if ( codigoSeguro.length === 0 ) {
            this.seguros.clear();
            return;
        }

        if ( this.seguros.length > 0 ) {
            this.seguros.controls.forEach( ( control, indexControl ) => {
                const seguroIndex = listaSeguros.findIndex( codigo => codigo === control.get( 'codigo' ).value );
                const seguro = listaSeguros.find( codigo => codigo === control.get( 'codigo' ).value );

                if ( seguroIndex !== -1 ) {
                    listaSeguros.splice( seguroIndex, 1 );
                }

                if ( seguro === undefined ) {
                    this.seguros.removeAt( indexControl );
                    listaSeguros.splice( seguroIndex, 1 );
                }
            } );
        }

        for ( const codigo of listaSeguros ) {
            const seguro = this.polizasYSegurosArray.find( poliza => poliza.codigo === codigo );

            if ( seguro !== undefined ) {
                this.seguros.push( this.fb.group(
                    {
                        nombre: [ seguro.nombre ],
                        codigo: [ seguro.codigo ],
                        tipoActualizacion: [ null, Validators.required ]
                    }
                ) )
            }
        }
    }
    // evalua tecla a tecla
    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    firstLetterUpperCase( texto:string ) {
        if ( texto !== undefined ) {
            return humanize.capitalize( String( texto ).toLowerCase() );
        }
    }

    deleteSeguro( index: number ) {
        this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
            .subscribe(
                value => {
                    if ( value === true ) {
                        const listSeguro: string[] = this.addressForm.get( 'polizasYSeguros' ).value;
                        const indexSeguro =listSeguro.findIndex( codigo => codigo === this.seguros.controls[ index ].get( 'codigo' ).value );
                        listSeguro.splice( indexSeguro, 1 );

                        this.addressForm.get( 'polizasYSeguros' ).setValue( listSeguro );
                        this.seguros.removeAt( index );
                        this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                    }
                }
            )
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

    onSubmit() {
        this.estaEditando = true;

        const contratoPolizaActualizacion = {
            contratoPolizaActualizacionId: this.addressForm.get( 'contratoPolizaActualizacionId' ).value,
            contratoPolizaId: 0,
            razonActualizacionCodigo: '',
            fechaExpedicionActualizacionPoliza: new Date(),
            contratoPolizaActualizacionSeguro: [
                {
                    contratoPolizaActualizacionId: this.addressForm.get( 'contratoPolizaActualizacionId' ).value,
                    tipoSeguroCodigo: '',
                    tieneFechaSeguro: true,
                    fechaSeguro: new Date(),
                    tieneFechaVigenciaAmparo: true,
                    fechaVigenciaAmparo: new Date(),
                    tieneFechaValorAmparo: true,
                    fechaValorAmparo: 0
                }
            ]
        }
    }

}
