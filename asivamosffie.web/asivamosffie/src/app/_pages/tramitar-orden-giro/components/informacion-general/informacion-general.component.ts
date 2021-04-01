import { MatDialog } from '@angular/material/dialog';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { ListaMediosPagoCodigo, MediosPagoCodigo, TipoSolicitud, TipoSolicitudes } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';

@Component({
  selector: 'app-informacion-general',
  templateUrl: './informacion-general.component.html',
  styleUrls: ['./informacion-general.component.scss']
})
export class InformacionGeneralComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Input() esExpensas: boolean;
    listaMediosPagoCodigo: ListaMediosPagoCodigo = MediosPagoCodigo;
    listaTipoSolicitud: TipoSolicitud = TipoSolicitudes;
    listaTipoSolicitudContrato: Dominio[] = [];
    listaMedioPago: Dominio[] = [];
    listaBancos: Dominio[] = [];
    valorTotalFactura = 0;
    ordenGiroId: 0;
    solicitudPagoFase: any;
    ordenGiroTercero: any;
    ordenGiroTerceroId: 0;
    addressForm: FormGroup;
    dataHistorial: any[] = [];
    dataSource = new MatTableDataSource();
    tablaHistorial = new MatTableDataSource();
    formObservacion: FormGroup = this.fb.group({
        tieneObservaciones: [ null, Validators.required ],
        observaciones: [ null, Validators.required ],
        fechaCreacion: [ null ]
    });
    displayedColumnsHistorial: string[]  = [
        'fechaRevision',
        'responsable',
        'historial'
    ];
    displayedColumns: string[] = [
        'drp',
        'numDrp',
        'valor',
        'saldo'
    ];
    dataTable: any[] = [
        {
          numDrp: 'IP_00090',
          valor: '$ 100.000.000',
          saldo: '$ 100.000.000'
        },
        {
          numDrp: 'IP_00123',
          valor: '$ 5.000.000',
          saldo: '$ 5.000.000'
        }
    ];
    editorStyle = {
        height: '100px'
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
        private fb: FormBuilder,
        private dialog: MatDialog,
        private commonSvc: CommonService )
    {
        this.commonSvc.listaTipoSolicitudContrato()
            .subscribe( response => this.listaTipoSolicitudContrato = response );
        this.addressForm = this.crearFormulario();
    }

    ngOnInit(): void {
        if ( this.solicitudPago.tipoSolicitudCodigo !== this.listaTipoSolicitud.expensas && this.solicitudPago.tipoSolicitudCodigo !== this.listaTipoSolicitud.otrosCostos ) {
            this.solicitudPagoFase = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0];

            this.solicitudPagoFase.solicitudPagoFaseCriterio.forEach( criterio => this.valorTotalFactura += criterio.valorFacturado );
        }
        this.dataHistorial = [
            {
                fechaCreacion: new Date(),
                responsable: 'Coordinador financiera',
                observacion: '<p>test historial</p>'
            }
        ];
        this.getDataTerceroGiro();
        this.dataSource = new MatTableDataSource( this.solicitudPago.contratoSon.valorFacturadoContrato );
        this.tablaHistorial = new MatTableDataSource( this.dataHistorial );
    }

    getDataTerceroGiro() {
        this.commonSvc.listaMediosPago()
        .subscribe( listaMediosPago => {
            this.listaMedioPago = listaMediosPago;

            this.commonSvc.listaBancos()
                .subscribe( listaBancos => {
                    this.listaBancos = listaBancos;

                    if ( this.solicitudPago.ordenGiro !== undefined ) {
                        this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;
            
                        if ( this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined ) {
                            if ( this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0 ) {
                                this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
                                this.ordenGiroTerceroId = this.ordenGiroTercero.ordenGiroTerceroId;

                                const medioPago = this.listaMedioPago.find( medio => medio.codigo === this.ordenGiroTercero.medioPagoGiroCodigo );

                                if ( medioPago !== undefined ) {
                                    this.addressForm.get( 'medioPagoGiroContrato' ).setValue( medioPago.codigo );
                                }

                                if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined ) {
                                    if ( this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0 ) {
                                        const ordenGiroTerceroTransferenciaElectronica = this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];

                                        this.addressForm.get( 'transferenciaElectronica' ).setValue(
                                            {
                                                ordenGiroTerceroId: this.ordenGiroTerceroId,
                                                ordenGiroTerceroTransferenciaElectronicaId: ordenGiroTerceroTransferenciaElectronica.ordenGiroTerceroTransferenciaElectronicaId,
                                                titularCuenta: ordenGiroTerceroTransferenciaElectronica.titularCuenta !== undefined ? ordenGiroTerceroTransferenciaElectronica.titularCuenta : '',
                                                titularNumeroIdentificacion: ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion !== undefined ? ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion : '',
                                                numeroCuenta: ordenGiroTerceroTransferenciaElectronica.numeroCuenta !== undefined ? ordenGiroTerceroTransferenciaElectronica.numeroCuenta : '',
                                                bancoCodigo: ordenGiroTerceroTransferenciaElectronica.bancoCodigo !== undefined ? ordenGiroTerceroTransferenciaElectronica.bancoCodigo : null,
                                                esCuentaAhorros: ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros !== undefined ? ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros : null
                                            }
                                        )
                                    }
                                }

                                if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined ) {
                                    if ( this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0 ) {
                                        const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];
                                        
                                        this.addressForm.get( 'chequeGerencia' ).setValue(
                                            {
                                                ordenGiroTerceroId: this.ordenGiroTerceroId,
                                                ordenGiroTerceroChequeGerenciaId: ordenGiroTerceroChequeGerencia.ordenGiroTerceroChequeGerenciaId,
                                                nombreBeneficiario: ordenGiroTerceroChequeGerencia.nombreBeneficiario !== undefined ? ordenGiroTerceroChequeGerencia.nombreBeneficiario : '',
                                                numeroIdentificacionBeneficiario: ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario !== undefined ? ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario : ''
                                            }
                                        )
                                    }
                                }
                            }
                        }
                    }
                } );
        } );
    }

    crearFormulario() {
        return this.fb.group({
            medioPagoGiroContrato: [null, Validators.required],
            transferenciaElectronica: this.fb.group( {
                ordenGiroTerceroId: [ 0 ],
                ordenGiroTerceroTransferenciaElectronicaId: [ 0 ],
                titularCuenta: [ '' ],
                titularNumeroIdentificacion: [ '' ],
                numeroCuenta: [ '' ],
                bancoCodigo: [ null ],
                esCuentaAhorros: [ null ]
            } ),
            chequeGerencia: this.fb.group( {
                ordenGiroTerceroId: [ 0 ],
                ordenGiroTerceroChequeGerenciaId: [ 0 ],
                nombreBeneficiario: [ '' ],
                numeroIdentificacionBeneficiario: [ '' ]
            } )
        })
    }

    getMedioPago( codigo: string ) {
        if ( this.listaMedioPago.length > 0 ) {
            const medioPago = this.listaMedioPago.find( medioPago => medioPago.codigo === codigo );

            if ( medioPago !== undefined ) {
                return medioPago.nombre;
            }
        }
    }

    getBanco( codigo: string ) {
        if ( this.listaBancos.length > 0 ) {
            const banco = this.listaMedioPago.find( banco => banco.codigo === codigo );

            if ( banco !== undefined ) {
                return banco.nombre;
            }
        }
    }

    getTipoSolicitudContrato( tipoSolicitudCodigo: string ) {
        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.otrosCostos ) {
            return 'Otros costos y servicios';
        }

        if ( tipoSolicitudCodigo === this.listaTipoSolicitud.expensas ) {
            return 'Expensas';
        }

        if ( this.listaTipoSolicitudContrato.length > 0 ) {
            const solicitud = this.listaTipoSolicitudContrato.find( solicitud => solicitud.codigo === tipoSolicitudCodigo );
            
            if ( solicitud !== undefined ) {
                return solicitud.nombre;
            }
        }
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

    openDialog( modalTitle: string, modalText: string ) {
        this.dialog.open( ModalDialogComponent, {
          width: '28em',
          data: { modalTitle, modalText }
        });
    }

    guardar() {
        if ( this.formObservacion.get( 'tieneObservaciones' ).value === false && this.formObservacion.get( 'observaciones' ).value !== null ) {
            this.formObservacion.get( 'observaciones' ).setValue( '' );
        }
        console.log( this.formObservacion );
    }

}
