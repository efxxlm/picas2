import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, Input, OnInit } from '@angular/core';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { MediosPagoCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ActivatedRoute, Params } from '@angular/router';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-form-orden-giro-seleccionada',
  templateUrl: './form-orden-giro-seleccionada.component.html',
  styleUrls: ['./form-orden-giro-seleccionada.component.scss']
})
export class FormOrdenGiroSeleccionadaComponent implements OnInit {
  @Input() ordenGiro: FormGroup;
  @Input() esVerDetalle: boolean;
  @Input() esRegistroNuevo: boolean;
  balanceFinancieroTrasladoId = 0;
  listaMediosPagoCodigo = MediosPagoCodigo;
  addressForm: FormGroup;
  listaModalidad: Dominio[] = [];
  listaMedioPago: Dominio[] = [];
  listaBancos: Dominio[] = [];
  solicitudPago: any;
  solicitudPagoFase: any;
  ordenGiroTercero: any;
  ordenGiroId: 0;
  ordenGiroTerceroId: 0;
  semaforoDetalle = 'sin-diligenciar';
  semaforoTerceroCausacion: string;
  semaforoDescuentos: string;
  solicitudPagoId: number;
  contrato: any;
  listaDetalleGiro: { contratacionProyectoId: number, llaveMen: string, fases: any[], semaforoDetalle: string }[] = [];
  valorTotalPagado = 0;

  constructor(
    private ordenPagoSvc: OrdenPagoService,
    private commonSvc: CommonService,
    private balanceSvc: FinancialBalanceService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private registrarPagosSvc: RegistrarRequisitosPagoService
  ) {
    this.addressForm = this.crearFormulario();
  }

  async ngOnInit() {
    this.route.params.subscribe((params: Params) => (this.solicitudPagoId = params.solicitudPagoId));
    this.listaModalidad = await this.commonSvc.modalidadesContrato().toPromise();
    this.listaMedioPago = await this.commonSvc.listaMediosPago().toPromise();
    this.listaBancos = await this.commonSvc.listaBancos().toPromise();
    if (this.solicitudPagoId) {
      this.solicitudPago = await this.ordenPagoSvc.getSolicitudPagoBySolicitudPagoId(this.solicitudPagoId).toPromise();

      this.getListaDetalleGiro(this.solicitudPago)
    } else {
      this.solicitudPago = await this.ordenPagoSvc
        .getSolicitudPagoBySolicitudPagoId(this.ordenGiro.get('solicitudPagoId').value)
        .toPromise();

      this.getListaDetalleGiro(this.solicitudPago)
    }

    this.solicitudPago.ordenGiro.ordenGiroDetalle[0].ordenGiroDetalleTerceroCausacion.forEach(element => {
      this.valorTotalPagado += element.valorNetoGiro
    });
    // console.log(this.solicitudPago);
    this.getDataTerceroGiro();
  }

  async getListaDetalleGiro(response) {
    this.solicitudPago = response;
    this.contrato = response[ 'contratoSon' ];
    console.log( this.solicitudPago );
    /*
        Se crea un arreglo de proyectos asociados a una fase y unos criterios que estan asociados a esa fase y al proyecto para
        el nuevo flujo de Orden de Giro el cual los acordeones de "Estrategia de pagos, Observaciones y Soporte de orden de giro" ya no son hijos del acordeon
        "Detalle de giro" y el detalle de giro se diligencia por proyectos el cual tendra como hijo directo las fases y los criterios asociados a esa fase y al proyecto.
    */
    // Peticion asincrona de los proyectos por contratoId
    const getProyectosByIdContrato: any[] = await this.registrarPagosSvc.getProyectosByIdContrato( this.solicitudPago.contratoId ).toPromise();
    const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[1];
    const solicitudPagoFase: any[] = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[ 0 ].solicitudPagoFase;

    LISTA_PROYECTOS.forEach( proyecto => {
        // Objeto Proyecto que se agregara al array listaDetalleGiro
        const PROYECTO = {
            semaforoDetalle: 'sin-diligenciar',
            contratacionProyectoId: proyecto.contratacionProyectoId,
            llaveMen: proyecto.llaveMen,
            fases: []
        }

        const listFase = solicitudPagoFase.filter( fase => fase.contratacionProyectoId === proyecto.contratacionProyectoId )
        if ( listFase.length > 0 ) {
            listFase.forEach( fase => {
                fase.estadoSemaforo = 'sin-diligenciar'
                fase.estadoSemaforoCausacion = 'sin-diligenciar'
            })
        }
        PROYECTO.fases = listFase

        if ( PROYECTO.fases.length > 0 ) {
            this.listaDetalleGiro.push( PROYECTO )
        }
    } )
  }
  crearFormulario() {
    return this.fb.group({
      medioPagoGiroContrato: [null, Validators.required],
      transferenciaElectronica: this.fb.group({
        ordenGiroTerceroId: [0],
        ordenGiroTerceroTransferenciaElectronicaId: [0],
        titularCuenta: [''],
        titularNumeroIdentificacion: [''],
        numeroCuenta: [''],
        bancoCodigo: [null],
        esCuentaAhorros: [null]
      }),
      chequeGerencia: this.fb.group({
        ordenGiroTerceroId: [0],
        ordenGiroTerceroChequeGerenciaId: [0],
        nombreBeneficiario: [''],
        numeroIdentificacionBeneficiario: ['']
      })
    });
  }

  getDataTerceroGiro() {
    this.commonSvc.listaMediosPago().subscribe(listaMediosPago => {
      this.listaMedioPago = listaMediosPago;

      this.commonSvc.listaBancos().subscribe(async listaBancos => {
        this.listaBancos = listaBancos;

        if (this.solicitudPago.ordenGiro !== undefined) {
          this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

          if (this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined) {
            if (this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0) {
              this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
              this.ordenGiroTerceroId = this.ordenGiroTercero.ordenGiroTerceroId;
              // Get data tercero de giro
              const medioPago = this.listaMedioPago.find(
                medio => medio.codigo === this.ordenGiroTercero.medioPagoGiroCodigo
              );

              if (medioPago !== undefined) {
                this.addressForm.get('medioPagoGiroContrato').setValue(medioPago.codigo);
              }

              if (this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined) {
                if (this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0) {
                  const ordenGiroTerceroTransferenciaElectronica =
                    this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];

                  this.addressForm.get('transferenciaElectronica').setValue({
                    ordenGiroTerceroId: this.ordenGiroTerceroId,
                    ordenGiroTerceroTransferenciaElectronicaId:
                      ordenGiroTerceroTransferenciaElectronica.ordenGiroTerceroTransferenciaElectronicaId,
                    titularCuenta:
                      ordenGiroTerceroTransferenciaElectronica.titularCuenta !== undefined
                        ? ordenGiroTerceroTransferenciaElectronica.titularCuenta
                        : '',
                    titularNumeroIdentificacion:
                      ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion !== undefined
                        ? ordenGiroTerceroTransferenciaElectronica.titularNumeroIdentificacion
                        : '',
                    numeroCuenta:
                      ordenGiroTerceroTransferenciaElectronica.numeroCuenta !== undefined
                        ? ordenGiroTerceroTransferenciaElectronica.numeroCuenta
                        : '',
                    bancoCodigo:
                      ordenGiroTerceroTransferenciaElectronica.bancoCodigo !== undefined
                        ? ordenGiroTerceroTransferenciaElectronica.bancoCodigo
                        : null,
                    esCuentaAhorros:
                      ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros !== undefined
                        ? ordenGiroTerceroTransferenciaElectronica.esCuentaAhorros
                        : null
                  });
                }
              }

              if (this.ordenGiroTercero.ordenGiroTerceroChequeGerencia !== undefined) {
                if (this.ordenGiroTercero.ordenGiroTerceroChequeGerencia.length > 0) {
                  const ordenGiroTerceroChequeGerencia = this.ordenGiroTercero.ordenGiroTerceroChequeGerencia[0];

                  this.addressForm.get('chequeGerencia').setValue({
                    ordenGiroTerceroId: this.ordenGiroTerceroId,
                    ordenGiroTerceroChequeGerenciaId: ordenGiroTerceroChequeGerencia.ordenGiroTerceroChequeGerenciaId,
                    nombreBeneficiario:
                      ordenGiroTerceroChequeGerencia.nombreBeneficiario !== undefined
                        ? ordenGiroTerceroChequeGerencia.nombreBeneficiario
                        : '',
                    numeroIdentificacionBeneficiario:
                      ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario !== undefined
                        ? ordenGiroTerceroChequeGerencia.numeroIdentificacionBeneficiario
                        : ''
                  });
                }
              }
            }
          }
        }
      });
    });
  }

  getModalidadContrato(modalidadCodigo: string) {
    if (this.listaModalidad.length > 0) {
      const modalidad = this.listaModalidad.find(modalidad => modalidad.codigo === modalidadCodigo);

      if (modalidad !== undefined) {
        return modalidad.nombre;
      }
    }
  }

  getMedioPago(codigo: string) {
    if (this.listaMedioPago.length > 0) {
      const medioPago = this.listaMedioPago.find(medioPago => medioPago.codigo === codigo);

      if (medioPago !== undefined) {
        return medioPago.nombre;
      }
    }
  }

  getBanco(codigo: string) {
    if (this.listaBancos.length > 0) {
      const banco = this.listaBancos.find(banco => banco.codigo === codigo);

      if (banco !== undefined) {
        return banco.nombre;
      }
    }
  }

  checkEstadoSemaforo() {
    if (this.semaforoTerceroCausacion === 'en-proceso' && this.semaforoDescuentos === 'en-proceso') {
      this.semaforoDetalle = 'en-proceso';
    }
    if (this.semaforoTerceroCausacion === 'en-proceso' && this.semaforoDescuentos === 'completo') {
      this.semaforoDetalle = 'en-proceso';
    }
    if (this.semaforoTerceroCausacion === 'completo' && this.semaforoDescuentos === 'en-proceso') {
      this.semaforoDetalle = 'en-proceso';
    }
    if (this.semaforoTerceroCausacion === 'completo' && this.semaforoDescuentos === 'sin-diligenciar') {
      this.semaforoDetalle = 'en-proceso';
    }
    if (this.semaforoTerceroCausacion === 'sin-diligenciar' && this.semaforoDescuentos === 'completo') {
      this.semaforoDetalle = 'en-proceso';
    }
    if (this.semaforoTerceroCausacion === 'completo' && this.semaforoDescuentos === 'completo') {
      this.semaforoDetalle = 'completo';
    }
    if (this.semaforoTerceroCausacion === 'en-proceso' && this.semaforoDescuentos === undefined) {
      this.semaforoDetalle = 'en-proceso';
    }
    if (this.semaforoTerceroCausacion === 'completo' && this.semaforoDescuentos === undefined) {
      this.semaforoDetalle = 'completo';
    }

    if (this.balanceFinancieroTrasladoId !== 0 && this.semaforoDetalle === 'completo') {
      this.balanceSvc.validateCompleteBalanceFinanciero(this.balanceFinancieroTrasladoId, 'True').subscribe();
    }

    if (this.balanceFinancieroTrasladoId !== 0 && this.semaforoDetalle === 'en-proceso') {
      this.balanceSvc.validateCompleteBalanceFinanciero(this.balanceFinancieroTrasladoId, 'False').subscribe();
    }
  }
}
