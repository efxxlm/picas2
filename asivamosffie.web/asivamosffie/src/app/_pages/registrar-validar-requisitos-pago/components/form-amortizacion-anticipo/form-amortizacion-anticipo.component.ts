import { Router } from '@angular/router';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { Validators, FormBuilder, FormControl } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';
import { Dominio } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-form-amortizacion-anticipo',
  templateUrl: './form-amortizacion-anticipo.component.html',
  styleUrls: ['./form-amortizacion-anticipo.component.scss']
})
export class FormAmortizacionAnticipoComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle = false;
    @Input() contrato: any;
    @Input() tieneObservacion: boolean;
    @Input() listaMenusId: any;
    @Input() faseCodigo: string;
    @Input() amortizacionAnticipoCodigo: string;
    @Input() tieneObservacionOrdenGiro: boolean;
    @Input() contratacionProyectoId: number;
    @Input() solicitudPagoCargarFormaPago: any;
    @Input() desabilitarAcordeonAmortizacionAnticipo: boolean;
    @Output() semaforoObservacion = new EventEmitter<boolean>();
    esPreconstruccion = true;
    solicitudPagoFase: any;
    solicitudPagoFaseAmortizacionId = 0;
    valorTotalDelContrato = 0;
    esAutorizar: boolean;
    observacion: any;
    solicitudPagoObservacionId = 0;
    addressForm = this.fb.group({
      porcentajeAmortizacion: [null, Validators.required],
      valorAmortizacion: [{ value: null, disabled: true }, Validators.required]
    });
    estaEditando = false;
    fasesContrato = TiposDeFase;
    valorFacturado = 0;

    valorPorAmortizar: FormControl;
    solicitudPagoRegistrarSolicitudPago: any;
    solicitudPagoRegistrarSolicitudPagoId: any;


    constructor(
        private fb: FormBuilder,
        private routes: Router,
        private dialog: MatDialog,
        private obsMultipleSvc: ObservacionesMultiplesCuService,
        private registrarPagosSvc: RegistrarRequisitosPagoService )
    {
        this.addressForm.get('porcentajeAmortizacion').valueChanges.subscribe(value => {
            if (this.valorFacturado > 0) {
                if ( value > this.valorFacturado ) {
                    this.addressForm.get('valorAmortizacion').setValue( null );
                }

                
                const exampleValue =  value / 100;
                const porcentajeCalculo = exampleValue * this.getProyectoId(this.contratacionProyectoId);
                // const valorAmortizacion = exampleValue * porcentajeCalculo;

                // console.log(porcentajeCalculo);
                this.addressForm.get('valorAmortizacion').setValue(porcentajeCalculo);
               
            } else {
                this.addressForm.get('valorAmortizacion').setValue( 0 );
            }
        });
    }

    ngOnInit(): void {
        this.getDataAmortizacion();
        this.valorPorAmortizar = new FormControl({value: this.getProyectoId(this.contratacionProyectoId), disabled: true}, [Validators.required, Validators.max(100)]);

        if(this.desabilitarAcordeonAmortizacionAnticipo) {
            this.addressForm.get('porcentajeAmortizacion').disable()
        }
    }

    getProyectoId(codigo: any) {
        if (this.contrato.vAmortizacionXproyecto.length > 0) {
          const proyectoId = this.contrato.vAmortizacionXproyecto.find(proyectoId => proyectoId.contratacionProyectoId === codigo);
          if (proyectoId !== undefined) {
            return proyectoId.valorPorAmortizar;
          }
        }
    }

    async getDataAmortizacion() {
        // Verificar la fase seleccionada en el proyecto
        if ( this.faseCodigo === this.fasesContrato.construccion ) {
            this.esPreconstruccion = false;
        }
        // Se obtiene la forma pago codigo dependiendo la fase seleccionada
        const FORMA_PAGO_CODIGO = this.esPreconstruccion === true ? this.solicitudPagoCargarFormaPago.fasePreConstruccionFormaPagoCodigo : this.solicitudPagoCargarFormaPago.faseConstruccionFormaPagoCodigo
        let LISTA_CRITERIOS_FORMA_PAGO = await this.registrarPagosSvc.getCriterioByFormaPagoCodigo( FORMA_PAGO_CODIGO ).toPromise()
        let criterioAnticipo: Dominio = null;
        criterioAnticipo = LISTA_CRITERIOS_FORMA_PAGO.find( value => value.nombre === 'Anticipo' )
        let faseCriterioAnticipo = undefined;
        let listaSolicitudesPago = [];

        if ( this.contrato.solicitudPago.length > 0 ) {
            listaSolicitudesPago = this.contrato.solicitudPago.filter( value => value.solicitudPagoId !== this.solicitudPago.solicitudPagoId )
        }

        if ( listaSolicitudesPago.length > 0 ) {
            listaSolicitudesPago.forEach( solicitud => {
                if ( solicitud.solicitudPagoRegistrarSolicitudPago !== undefined && solicitud.solicitudPagoRegistrarSolicitudPago.length > 0 ) {
                    const solicitudPagoRegistrarSolicitudPago = solicitud.solicitudPagoRegistrarSolicitudPago[ 0 ]

                    if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
                        solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.forEach( fase => {
                            if ( fase.solicitudPagoFaseCriterio.length > 0 ) {
                                const faseCriterioFind = fase.solicitudPagoFaseCriterio.find( faseCriterio => faseCriterio.tipoCriterioCodigo === criterioAnticipo.codigo )

                                if ( faseCriterioFind !== undefined ) {
                                    faseCriterioAnticipo = faseCriterioFind
                                }
                            }
                        } )
                    }
                }
            } )
        }

        if ( faseCriterioAnticipo !== undefined ) {
            this.valorFacturado = faseCriterioAnticipo.valorFacturado
        }

        if (this.contrato.contratacion.disponibilidadPresupuestal.length > 0) {
            this.contrato.contratacion.disponibilidadPresupuestal.forEach( ddp => this.valorTotalDelContrato += ddp.valorSolicitud );
        }
      
        console.log('solicitudPago: ', this.solicitudPago);
        
        if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0] !== undefined ) {
            if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase !== undefined && this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ 0 ] !== undefined ) {
                if (this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0) {
                    for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
                        console.log('solicitudPagoFase: ', solicitudPagoFase);
                        
                        if (solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
                            this.solicitudPagoFase = solicitudPagoFase;
                        }
                    }
                }
          
                if ( this.solicitudPagoFase !== undefined ) {
                    if ( this.solicitudPagoFase.solicitudPagoFaseAmortizacion.length > 0 ) {
                        const solicitudPagoFaseAmortizacion = this.solicitudPagoFase.solicitudPagoFaseAmortizacion[0];
                        this.solicitudPagoFaseAmortizacionId = solicitudPagoFaseAmortizacion.solicitudPagoFaseAmortizacionId;
                        this.estaEditando = true;
    
                        this.addressForm.markAllAsTouched();
                        this.addressForm.setValue(
                            {
                                porcentajeAmortizacion: solicitudPagoFaseAmortizacion.porcentajeAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.porcentajeAmortizacion : null,
                                valorAmortizacion: solicitudPagoFaseAmortizacion.valorAmortizacion !== undefined ? solicitudPagoFaseAmortizacion.valorAmortizacion : null
                            }
                        );
              
                        if (this.esVerDetalle === false) {
                            // Get observacion CU autorizar solicitud de pago 4.1.9
                            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                this.listaMenusId.autorizarSolicitudPagoId,
                                this.contrato.solicitudPagoOnly.solicitudPagoId,
                                this.solicitudPagoFaseAmortizacionId,
                                this.amortizacionAnticipoCodigo
                            )
                            .subscribe(response => {
                                const observacion = response.find(obs => obs.archivada === false);
                                if ( observacion !== undefined ) {
                                    this.esAutorizar = true;
                                    this.observacion = observacion;
                                
                                    if (this.observacion.tieneObservacion === true) {
                                        this.semaforoObservacion.emit(true);
                                        this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                    }
                                }
                            });
              
                            // Get observacion CU verificar solicitud de pago 4.1.8
                            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
                                this.listaMenusId.aprobarSolicitudPagoId,
                                this.contrato.solicitudPagoOnly.solicitudPagoId,
                                this.solicitudPagoFaseAmortizacionId,
                                this.amortizacionAnticipoCodigo
                            )
                            .subscribe(response => {
                                const observacion = response.find(obs => obs.archivada === false);
                                if ( observacion !== undefined ) {
                                    this.esAutorizar = false;
                                    this.observacion = observacion;
    
                                    if (this.observacion.tieneObservacion === true) {
                                        this.semaforoObservacion.emit(true);
                                        this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                                    }
                                }
                            });
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
        } );
    }

    numberValidate(value: any) {
        if ( value > 100 ) {
            this.addressForm.get('porcentajeAmortizacion').setValue(100);
        }
        if ( value < 0 ) {
            this.addressForm.get('porcentajeAmortizacion').setValue(0);
        }
    }

    onSubmit() {
        this.estaEditando = true;
        this.addressForm.markAllAsTouched();
        const solicitudPagoFaseCriterio = [];

        this.solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0]
        this.solicitudPagoRegistrarSolicitudPagoId = this.solicitudPagoRegistrarSolicitudPago.solicitudPagoRegistrarSolicitudPagoId

        if ( this.faseCodigo === this.fasesContrato.preConstruccion ) {
            if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                const solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )
                const solicitudPagoFaseIndex = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.findIndex( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )

                if ( solicitudPagoFase !== undefined ) {
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].solicitudPagoFaseCriterio = solicitudPagoFaseCriterio
                    // this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].esAnticipio = esAnticipio
                } else {
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.push(
                        {
                            solicitudPagoFaseId: 0,
                            esPreconstruccion: this.esPreconstruccion,
                            contratacionProyectoId: this.contratacionProyectoId
                        }
                    )
                }
            } else {
                this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase = [
                    {
                        solicitudPagoFaseId: 0,
                        esPreconstruccion: this.esPreconstruccion,
                        contratacionProyectoId: this.contratacionProyectoId
                    }
                ]
            }
        }

        if ( this.faseCodigo === this.fasesContrato.construccion ) {
            if ( this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0 ) {
                const solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.find( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )
                const solicitudPagoFaseIndex = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.findIndex( solicitudPagoFase => solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId )

                if ( solicitudPagoFase !== undefined ) {
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].solicitudPagoFaseCriterio = solicitudPagoFaseCriterio
                    // this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[ solicitudPagoFaseIndex ].esAnticipio = esAnticipio
                } else {
                    // console.log( 'Test' )
                    this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.push(
                        {
                            solicitudPagoFaseId: 0,
                            esPreconstruccion: this.esPreconstruccion,
                            contratacionProyectoId: this.contratacionProyectoId,
                            solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId
                        }
                    )
                }
            } else {
                this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase = [
                    {
                        solicitudPagoFaseId: 0,
                        esPreconstruccion: this.esPreconstruccion,
                        contratacionProyectoId: this.contratacionProyectoId,
                        solicitudPagoRegistrarSolicitudPagoId: this.solicitudPagoRegistrarSolicitudPagoId
                    }
                ]
            }
        }
        const solicitudPagoFaseAmortizacion = [
            {
                solicitudPagoFaseAmortizacionId: this.solicitudPagoFaseAmortizacionId,
                solicitudPagoFaseId: this.solicitudPagoFase ? this.solicitudPagoFase.solicitudPagoFaseId : null,
                porcentajeAmortizacion: this.addressForm.get('porcentajeAmortizacion').value,
                valorAmortizacion: this.addressForm.get('valorAmortizacion').value
            }
        ];
    
        for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
            if (solicitudPagoFase.esPreconstruccion === false) {
                solicitudPagoFase.solicitudPagoFaseAmortizacion = solicitudPagoFaseAmortizacion;
            }
        }
    
        console.log(this.solicitudPago)
        this.registrarPagosSvc.createEditNewPayment(this.solicitudPago)
            .subscribe(
                response => {
                    this.openDialog('', `<b>${response.message}</b>`);

                    if ( this.observacion !== undefined ) {
                        this.observacion.archivada = !this.observacion.archivada;
                        this.obsMultipleSvc.createUpdateSolicitudPagoObservacion(this.observacion).subscribe();
                    }

                    this.registrarPagosSvc.getValidateSolicitudPagoId(this.solicitudPago.solicitudPagoId).subscribe(() => {
                        this.routes.navigateByUrl('/', { skipLocationChange: true })
                        .then( () => this.routes.navigate([ '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId ]) );
                    });
                },
                err => this.openDialog('', `<b>${err.message}</b>`)
            );
    }

}
