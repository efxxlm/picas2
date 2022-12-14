import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActualizarPolizasService } from 'src/app/core/_services/actualizarPolizas/actualizar-polizas.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-accord-vigencias-valor',
  templateUrl: './accord-vigencias-valor.component.html',
  styleUrls: ['./accord-vigencias-valor.component.scss']
})
export class AccordVigenciasValorComponent implements OnInit {

    @Input() contratoPoliza: any;
    @Input() esVerDetalle: boolean;
    minDate = new Date();
    contratoPolizaActualizacion: any;
    contratoPolizaActualizacionSeguro: any;
    polizasYSegurosArray : Dominio[] = [];
    addressForm = this.fb.group(
        {
            seguros: this.fb.array( [] )
        }
    );
    estaEditando = false;

    get seguros() {
        return this.addressForm.get( 'seguros' ) as FormArray;
    }

    constructor(
        private dialog: MatDialog,
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private routes: Router,
        private actualizarPolizaSvc: ActualizarPolizasService )
    { }

    ngOnInit(): void {
        this.getVigenciaValor();
    }

    async getVigenciaValor() {
        this.polizasYSegurosArray = await this.commonSvc.listaGarantiasPolizas().toPromise();

        if ( this.contratoPoliza.contratoPolizaActualizacion !== undefined ) {
            if ( this.contratoPoliza.contratoPolizaActualizacion.length > 0 ) {
                this.contratoPolizaActualizacion = this.contratoPoliza.contratoPolizaActualizacion[ 0 ];

                if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro !== undefined ) {
                    if ( this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro.length > 0 ) {
                        this.contratoPolizaActualizacionSeguro = this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro;
                        const polizaGarantia: any[] = this.contratoPoliza.polizaGarantia.length > 0 ? this.contratoPoliza.polizaGarantia : [];

                        for ( const seguro of this.contratoPolizaActualizacionSeguro ) {
                            const seguroPoliza = polizaGarantia.find( seguroPoliza => seguroPoliza.tipoGarantiaCodigo === seguro.tipoSeguroCodigo );

                            let semaforo = 'sin-diligenciar';

                            if ( seguro.registroCompletoSeguro === false ) {
                                semaforo = 'en-proceso';
                            }
                            if ( seguro.registroCompletoSeguro === true ) {
                                semaforo = 'completo';
                            }

                            if ( seguroPoliza !== undefined ) {
                                this.seguros.push( this.fb.group(
                                    {
                                        semaforo,
                                        seguroPoliza,
                                        contratoPolizaActualizacionId: [ seguro.contratoPolizaActualizacionId !== undefined ? seguro.contratoPolizaActualizacionId : null, Validators.required ],
                                        contratoPolizaActualizacionSeguroId: [ seguro.contratoPolizaActualizacionSeguroId !== undefined ? seguro.contratoPolizaActualizacionSeguroId : null, Validators.required ],
                                        nombre: [ this.polizasYSegurosArray.find( poliza => poliza.codigo === seguro.tipoSeguroCodigo ).nombre ],
                                        codigo: [ seguro.tipoSeguroCodigo ],
                                        tieneSeguro: [ seguro.tieneFechaSeguro ],
                                        fechaSeguro: [ seguro.fechaSeguro !== undefined ? new Date( seguro.fechaSeguro ) : null, Validators.required ],
                                        tieneFechaAmparo: [ seguro.tieneFechaVigenciaAmparo ],
                                        fechaAmparo: [ seguro.fechaVigenciaAmparo !== undefined ? new Date( seguro.fechaVigenciaAmparo ) : null, Validators.required ],
                                        tieneValorAmparo: [ seguro.tieneValorAmparo ],
                                        valorAmparo: [ seguro.valorAmparo !== undefined ? seguro.valorAmparo : null, Validators.required ]
                                    }
                                ) )
                            }
                        }
                    }
                }
            }
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    onSubmit() {
        this.estaEditando = true;
        const listaContratoPolizaActualizacionSeguro = [ ...this.contratoPolizaActualizacionSeguro ];
        const actualizacionDiligenciadas = [];


        this.seguros.controls.forEach( control => {
            const seguro = listaContratoPolizaActualizacionSeguro.find( seguro => seguro.tipoSeguroCodigo === control.get( 'codigo' ).value && seguro.contratoPolizaActualizacionSeguroId === control.get( 'contratoPolizaActualizacionSeguroId' ).value);
            console.log(listaContratoPolizaActualizacionSeguro);
            if ( seguro !== undefined ) {
                seguro.fechaSeguro = control.get( 'fechaSeguro' ).value !== null ? new Date( control.get( 'fechaSeguro' ).value ).toISOString() : control.get( 'fechaSeguro' ).value;
                seguro.fechaVigenciaAmparo = control.get( 'fechaAmparo' ).value !== null ? new Date( control.get( 'fechaAmparo' ).value ).toISOString() : control.get( 'fechaAmparo' ).value;
                seguro.valorAmparo = control.get( 'valorAmparo' ).value;

                actualizacionDiligenciadas.push( seguro );
            }
        } );

        this.contratoPolizaActualizacion.contratoPolizaActualizacionSeguro = actualizacionDiligenciadas;

        this.actualizarPolizaSvc.createorUpdateCofinancing( this.contratoPolizaActualizacion )
            .subscribe(
                response => {
                    this.openDialog( '', `<b>${ response.message }</b>` );
                    this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                        () => this.routes.navigate(
                            [
                                '/registrarActualizacionesPolizasYGarantias/verDetalleEditarPoliza',  response.data
                            ]
                        )
                    );
                },
                err => this.openDialog( '', `<b>${ err.message }</b>` )
            );
    }

}
