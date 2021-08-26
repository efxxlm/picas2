import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrdenPagoService } from 'src/app/core/_services/ordenPago/orden-pago.service';
import { TipoSolicitudCodigo } from './enums-plantilla/tipo-solicitud.enums';
import { FormBuilder, FormArray, Validators } from '@angular/forms';
import humanize from 'humanize-plus';
import { MediosPagoCodigo } from 'src/app/_interfaces/estados-solicitudPago-ordenGiro.interface';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';

@Component({
  selector: 'app-plantilla',
  templateUrl: './plantilla.component.html',
  styleUrls: ['./plantilla.component.scss']
})
export class PlantillaComponent implements OnInit {
  solicitudPago = undefined;
  ordenGiroTercero = undefined;
  tipoSolicitudCodigo = TipoSolicitudCodigo;
  listaMediosPagoCodigo = MediosPagoCodigo;
  bancosArray: Dominio[] = [];
  listaMedioPago: Dominio[] = [];
  listaBancos: Dominio[] = [];
  criterioPreconstruccion: any[];
  criterioConstruccion: any[];
  terceroGiroForm = this.fb.group({
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
  fasePreConstruccionFormaPagoCodigo: any;
  conceptosDePago: any[] = [];
  listaDetalleGiro: {
    contratacionProyectoId: number;
    llaveMen: string;
    semaforoDetalle: string;
    listaTerceroCausacion: any[];
  }[] = [];
  formOrigen = this.fb.group({
    aportantes: this.fb.array([])
  });
  infoPlantilla: any;
  tablaOrdenGiro = [];
  valorNetoGiro = 0;
  descuentos = { valorTotal: 0, retegarantia: 0, ans: 0, otrosDescuentos: 0 };

  get aportantes() {
    return this.formOrigen.get('aportantes') as FormArray;
  }

  constructor(
    private activatedRoute: ActivatedRoute,
    private commonSvc: CommonService,
    private ordenGiroSvc: OrdenPagoService,
    private fb: FormBuilder,
    private registrarPagosSvc: RegistrarRequisitosPagoService
  ) {}

  ngOnInit(): void {
    this.getOrdenGiro();
  }

  async getOrdenGiro() {
    let solicitudPago;
    solicitudPago = await this.ordenGiroSvc
      .getSolicitudPagoBySolicitudPagoId(this.activatedRoute.snapshot.params.id)
      .toPromise();
    console.log(solicitudPago);
    // solicitudPago.contratoSon.solicitudPagoOnly.forEach(
    //   criterio => (this.valorNetoGiro += criterio.valorFacturado)
    // );
    this.valorNetoGiro = solicitudPago.contratoSon.solicitudPagoOnly.valorFacturado;
    if (
      solicitudPago.contratoSon.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0]
        .solicitudPagoFaseFacturaDescuento.length > 0
    ) {
      solicitudPago.contratoSon.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0].solicitudPagoFaseFacturaDescuento.forEach(
        descuento => (this.valorNetoGiro -= descuento.valorDescuento)
      );
    }
    for (
      let i = 0;
      i <
      solicitudPago.contratoSon.contratacion.contratacionProyecto[0].contratacionProyectoAportante[0]
        .cofinanciacionAportante.fuenteFinanciacion.length;
      i++
    ) {
      const element =
        solicitudPago.contratoSon.contratacion.contratacionProyecto[0].contratacionProyectoAportante[0]
          .cofinanciacionAportante.fuenteFinanciacion[i];

      let datosTabla = {
        consecutivoOrigen: solicitudPago.ordenGiro.consecutivoOrigen,
        nombreCuentaBanco: element.cuentaBancaria[0].nombreCuentaBanco,
        numeroCuentaBanco: element.cuentaBancaria[0].numeroCuentaBanco,
        bancoCodigo: element.cuentaBancaria[0].bancoCodigo,
        tipoCuentaCodigo: element.cuentaBancaria[0].tipoCuentaCodigo,
        codigoSifi: element.cuentaBancaria[0].codigoSifi,
        numeroIdentificacion: solicitudPago.contratoSon.contratacion.contratista.numeroIdentificacion,
        nombre: solicitudPago.contratoSon.contratacion.contratista.nombre,
        numero: solicitudPago.solicitudPagoFactura[0].numero,
        conceptoPagoCriterio:
          solicitudPago.contratoSon.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0]
            .solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioConceptoPago[0].conceptoPagoCriterio,
        valorFacturadoConcepto:
          solicitudPago.contratoSon.solicitudPagoOnly.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase[0]
            .solicitudPagoFaseCriterio[0].solicitudPagoFaseCriterioConceptoPago[0].valorFacturadoConcepto
      };

      this.tablaOrdenGiro.push(datosTabla);
    }
    for (let i = 0; i < solicitudPago.ordenGiro.ordenGiroTercero.length; i++) {
      const element = solicitudPago.ordenGiro.ordenGiroTercero[i];

      this.tablaOrdenGiro[i].esCuentaAhorros = element.ordenGiroTerceroTransferenciaElectronica[0].esCuentaAhorros
        ? 'Ahorros'
        : 'Corriente';
      this.tablaOrdenGiro[i].bancoCodigo = element.ordenGiroTerceroTransferenciaElectronica[0].bancoCodigo;
      this.tablaOrdenGiro[i].numeroCuenta = element.ordenGiroTerceroTransferenciaElectronica[0].numeroCuenta;
      this.tablaOrdenGiro[i].titularNumeroIdentificacion =
        element.ordenGiroTerceroTransferenciaElectronica[0].titularNumeroIdentificacion;
      this.tablaOrdenGiro[i].titularCuenta = element.ordenGiroTerceroTransferenciaElectronica[0].titularCuenta;
    }
    this.bancosArray = await this.commonSvc.listaBancos().toPromise();
    this.solicitudPago = solicitudPago;

    this.getProyectos();
    this.getOrigen();
    this.getDataTerceroGiro();
    this.getInfoPlantilla(this.solicitudPago.ordenGiroId);
  }

  getInfoPlantilla(ordenGiroId) {
    this.ordenGiroSvc.getInfoPlantilla(ordenGiroId).subscribe(response => {
      this.infoPlantilla = response;
      console.log(this.infoPlantilla);
      this.infoPlantilla.forEach(element => {
        if (element[0].nombre === 'ANS') this.descuentos.retegarantia += element[0].valorDescuento;
        else if (element[0].nombre === 'Retegarantia') this.descuentos.ans += element[0].valorDescuento;
        else this.descuentos.otrosDescuentos += element[0].valorDescuento;

        this.descuentos.valorTotal += element[0].valorDescuento;
      });
    });
  }

  async getProyectos() {
    // Peticion asincrona de los proyectos por contratoId
    const getProyectosByIdContrato: any[] = await this.registrarPagosSvc
      .getProyectosByIdContrato(this.solicitudPago.contratoId)
      .toPromise();
    const LISTA_PROYECTOS: any[] = getProyectosByIdContrato[1];
    const terceroCausacion = this.solicitudPago.ordenGiro.ordenGiroDetalle[0].ordenGiroDetalleTerceroCausacion;

    if (this.solicitudPago.contratoSon.solicitudPago.length > 1) {
      this.fasePreConstruccionFormaPagoCodigo =
        this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
    } else {
      this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
    }

    this.criterioPreconstruccion = await this.registrarPagosSvc
      .getCriterioByFormaPagoCodigo(this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo)
      .toPromise();
    this.criterioConstruccion = await this.registrarPagosSvc
      .getCriterioByFormaPagoCodigo(this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo)
      .toPromise();

    for (const proyecto of LISTA_PROYECTOS) {
      // Objeto Proyecto que se agregara al array listaDetalleGiro
      const PROYECTO = {
        semaforoDetalle: 'sin-diligenciar',
        contratacionProyectoId: proyecto.contratacionProyectoId,
        llaveMen: proyecto.llaveMen,
        listaTerceroCausacion: []
      };

      const listaTerceroCausacion = terceroCausacion.filter(
        tercero => tercero.contratacionProyectoId === proyecto.contratacionProyectoId
      );

      if (listaTerceroCausacion.length > 0) {
        for (const tercero of listaTerceroCausacion) {
          const listaConceptos = [];
          const conceptos = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo(
            tercero.tipoPagoCodigo
          );
          this.conceptosDePago.push(...conceptos);
          for (const ordenGiroDetalleTerceroCausacionAportante of tercero.ordenGiroDetalleTerceroCausacionAportante) {
            const conceptoFind = listaConceptos.find(
              concepto => concepto === ordenGiroDetalleTerceroCausacionAportante.conceptoPagoCodigo
            );

            if (conceptoFind === undefined)
              listaConceptos.push(ordenGiroDetalleTerceroCausacionAportante.conceptoPagoCodigo);
          }

          tercero.conceptos = listaConceptos;

          PROYECTO.listaTerceroCausacion = listaTerceroCausacion;
        }
        this.listaDetalleGiro.push(PROYECTO);
      }
    }
    console.log(this.listaDetalleGiro);
  }

  getDataTerceroGiro() {
    this.commonSvc.listaMediosPago().subscribe(listaMediosPago => {
      this.listaMedioPago = listaMediosPago;

      this.commonSvc.listaBancos().subscribe(async listaBancos => {
        this.listaBancos = listaBancos;

        if (this.solicitudPago.ordenGiro !== undefined) {
          if (this.solicitudPago.ordenGiro.ordenGiroTercero !== undefined) {
            if (this.solicitudPago.ordenGiro.ordenGiroTercero.length > 0) {
              this.ordenGiroTercero = this.solicitudPago.ordenGiro.ordenGiroTercero[0];
              // Get data tercero de giro
              const medioPago = this.listaMedioPago.find(
                medio => medio.codigo === this.ordenGiroTercero.medioPagoGiroCodigo
              );

              if (medioPago !== undefined) {
                this.terceroGiroForm.get('medioPagoGiroContrato').setValue(medioPago.codigo);
              }

              if (this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica !== undefined) {
                if (this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica.length > 0) {
                  const ordenGiroTerceroTransferenciaElectronica =
                    this.ordenGiroTercero.ordenGiroTerceroTransferenciaElectronica[0];

                  this.terceroGiroForm.get('transferenciaElectronica').setValue({
                    ordenGiroTerceroId: 0,
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

                  this.terceroGiroForm.get('chequeGerencia').setValue({
                    ordenGiroTerceroId: 0,
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

  async getOrigen() {
    const dataAportantes = await this.ordenGiroSvc.getAportantes(this.solicitudPago);

    // Get IDs
    if (this.solicitudPago.ordenGiro !== undefined) {
      const listaAportantes = [];

      if (this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined) {
        if (this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0) {
          const ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];

          if (ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined) {
            if (ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0) {
              const ordenGiroDetalleTerceroCausacion = ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;

              ordenGiroDetalleTerceroCausacion.forEach(terceroCausacion => {
                if (terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.length > 0) {
                  terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.forEach(aportante => {
                    const aportanteFind = listaAportantes.find(value => value.aportanteId === aportante.aportanteId);

                    if (aportanteFind === undefined) {
                      listaAportantes.push(aportante);
                    }
                  });
                }
              });

              for (const aportante of listaAportantes) {
                const nombreAportante = dataAportantes.listaNombreAportante.find(
                  nombreAportante => nombreAportante.cofinanciacionAportanteId === aportante.aportanteId
                );
                const tipoAportante = dataAportantes.listaTipoAportante.find(
                  tipoAportante => tipoAportante.dominioId === nombreAportante.tipoAportanteId
                );
                const fuente = await this.ordenGiroSvc
                  .getFuentesDeRecursosPorAportanteId(nombreAportante.cofinanciacionAportanteId)
                  .toPromise();
                const fuenteRecurso = fuente.find(fuenteValue => fuenteValue.codigo === aportante.fuenteRecursoCodigo);
                const cuentaBancaria = () => {
                  if (aportante.fuenteFinanciacion.cuentaBancaria.length > 1) {
                    if (aportante.cuentaBancariaId !== undefined) {
                      const cuenta = aportante.fuenteFinanciacion.cuentaBancaria.find(
                        cuenta => cuenta.cuentaBancariaId === aportante.cuentaBancariaId
                      );

                      if (cuenta !== undefined) {
                        return cuenta;
                      } else {
                        return null;
                      }
                    } else {
                      return null;
                    }
                  } else {
                    return aportante.fuenteFinanciacion.cuentaBancaria[0];
                  }
                };

                this.aportantes.push(
                  this.fb.group({
                    tipoAportante: [tipoAportante],
                    nombreAportante: [nombreAportante],
                    fuente: [fuenteRecurso],
                    listaCuentaBancaria: [aportante.fuenteFinanciacion.cuentaBancaria],
                    cuentaBancariaId: [cuentaBancaria(), Validators.required]
                  })
                );
                console.log(this.aportantes);
              }
            }
          }
        }
      }
    }
  }

  firstLetterUpperCase(texto: string) {
    if (texto !== undefined) {
      return humanize.capitalize(String(texto).toLowerCase());
    }
  }

  getBanco(codigo: string) {
    if (this.bancosArray.length > 0) {
      const banco = this.bancosArray.find(banco => banco.codigo === codigo);
      if (banco !== undefined) {
        return banco.nombre;
      }
    }
  }

  getCriterio(codigo: string) {
    if (this.criterioPreconstruccion.length > 0 || this.criterioConstruccion.length > 0) {
      const criterio =
        this.criterioPreconstruccion.find(criterio => criterio.codigo === codigo) ||
        this.criterioConstruccion.find(criterio => criterio.codigo === codigo);

      if (criterio !== undefined) {
        return criterio.nombre;
      }
    }
  }

  getConceptos(codigo: string) {
    if (this.conceptosDePago.length > 0) {
      const concepto = this.conceptosDePago.find(concepto => concepto.codigo === codigo);

      if (concepto !== undefined) return concepto.nombre;
    }
  }

  getHtmlToPdf() {
    window.print();

    // const pdfHTML = document.getElementById('pdf').innerHTML;
    // const w = window.open();
    // w.document.write(pdfHTML);
    // w.document.close();
    // w.focus();
    // w.print();
    // w.close();
    // return true;

    // const pdf = {
    //   EsHorizontal: true,
    //   MargenArriba: 2,
    //   MargenAbajo: 2,
    //   MargenDerecha: 2,
    //   MargenIzquierda: 2,
    //   Contenido: pdfHTML
    // };

    // this.commonSvc.GetHtmlToPdf(pdf).subscribe(
    //   response => {
    //     const documento = `OrdernGiro.pdf`;
    //     const text = documento,
    //       blob = new Blob([response], { type: 'application/pdf' }),
    //       anchor = document.createElement('a');
    //     anchor.download = documento;
    //     anchor.href = window.URL.createObjectURL(blob);
    //     anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
    //     anchor.click();
    //   },
    //   e => {
    //     console.log(e);
    //   }
    // );
  }
}
