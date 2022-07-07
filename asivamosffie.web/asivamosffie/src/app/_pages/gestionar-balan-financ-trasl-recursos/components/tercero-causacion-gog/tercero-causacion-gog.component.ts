import { filter } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { OrdenPagoService } from './../../../../core/_services/ordenPago/orden-pago.service';
import { TipoAportanteDominio, TipoAportanteCodigo, ListaMenu, ListaMenuId, TipoObservaciones, TipoObservacionesCodigo } from './../../../../_interfaces/estados-solicitudPago-ordenGiro.interface';
import { CommonService, Dominio } from './../../../../core/_services/common/common.service';
import { Component, Input, OnInit, OnChanges, SimpleChanges, Output, EventEmitter } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegistrarRequisitosPagoService } from 'src/app/core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import humanize from 'humanize-plus';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesOrdenGiroService } from 'src/app/core/_services/observacionesOrdenGiro/observaciones-orden-giro.service';
import { TipoTrasladoCodigo } from 'src/app/_interfaces/balance-financiero.interface';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ValidacionesLineaPagoService } from 'src/app/core/_services/validacionesLineaPago/validaciones-linea-pago.service';

@Component({
    selector: 'app-tercero-causacion-gog',
    templateUrl: './tercero-causacion-gog.component.html',
    styleUrls: ['./tercero-causacion-gog.component.scss']
})
export class TerceroCausacionGogComponent implements OnInit {

    @Input() solicitudPago: any;
    @Input() esVerDetalle: boolean;
    @Input() esRegistroNuevo: boolean;
    @Input() esPreconstruccion: boolean;
    @Input() solicitudPagoFase: any;
    @Output() estadoSemaforo = new EventEmitter<string>();
    @Input() contratacionProyectoId: number;
    tipoTrasladoCodigo = TipoTrasladoCodigo;
    balanceFinanciero: any;
    balanceFinancieroId = 0;
    balanceFinancieroTrasladoValor: any;
    balanceFinancieroTraslado: any;
    balanceFinancieroTrasladoId = 0;
    usosDrp: any[];
    usosFacturados: any[];

    listaMenu: ListaMenu = ListaMenuId;
    tipoObservaciones: TipoObservaciones = TipoObservacionesCodigo;
    addressForm: FormGroup;
    tipoDescuentoArray: Dominio[] = [];
    listaTipoDescuento: Dominio[] = [];
    listaCriterios: Dominio[] = [];
    listaFuenteTipoFinanciacion: Dominio[] = [];
    cantidadAportantes: number;
    solicitudPagoFaseCriterio: any;
    solicitudPagoFaseFacturaDescuento: any;
    fasePreConstruccionFormaPagoCodigo: any;
    ordenGiroDetalle: any;
    ordenGiroDetalleTerceroCausacion: any[];
    variosAportantes: boolean;
    estaEditando = false;
    valorNetoGiro = 0;
    ordenGiroId = 0;
    ordenGiroDetalleId = 0;

    // Get formArray de addressForm
    get criterios() {
        return this.addressForm.get('criterios') as FormArray;
    }

    getConceptos(index: number) {
        return this.criterios.controls[index].get('conceptos') as FormArray;
    }

    getAportantes(index: number, jIndex: number) {
        return this.getConceptos(index).controls[jIndex].get('aportantes') as FormArray;
    }

    getDescuentos(index: number, jIndex: number) {
        return this.getConceptos(index).controls[jIndex].get('descuento').get('descuentos') as FormArray;
    }

    getAportanteDescuentos(index: number, jIndex: number, kIndex: number) {
        return this.getDescuentos(index, jIndex).controls[kIndex].get('aportantesDescuento') as FormArray;
    }

    constructor(
        private fb: FormBuilder,
        private commonSvc: CommonService,
        private dialog: MatDialog,
        private routes: Router,
        private registrarPagosSvc: RegistrarRequisitosPagoService,
        private ordenGiroSvc: OrdenPagoService,
        private obsOrdenGiro: ObservacionesOrdenGiroService,
        private _balanceSvc: FinancialBalanceService,
        private _activatedRoute: ActivatedRoute,
        private validacionSvc: ValidacionesLineaPagoService,) {
        this.commonSvc.listaFuenteTipoFinanciacion()
            .subscribe(listaFuenteTipoFinanciacion => this.listaFuenteTipoFinanciacion = listaFuenteTipoFinanciacion);
        this.crearFormulario();
        this.validacionSvc.validacionFacturadosODG().subscribe((response) => {
            this.usosDrp = response?.usosDrp;
            this.usosFacturados = response?.usosFacturados;
        });
    }

    ngOnInit(): void {
        this.getTerceroCausacion();
    }

    crearFormulario() {
        this.addressForm = this.fb.group(
            {
                criterios: this.fb.array([])
            }
        );
    }

    async getTerceroCausacion() {
        this.tipoDescuentoArray = await this.commonSvc.listaDescuentosOrdenGiro().toPromise();
        this.listaTipoDescuento = await this.commonSvc.listaDescuentosOrdenGiro().toPromise();
        this.balanceFinanciero = await this._balanceSvc.getBalanceFinanciero(Number(this._activatedRoute.snapshot.paramMap.get('id'))).toPromise();
        // Get IDs
        if (this.solicitudPago.ordenGiro !== undefined) {
            this.ordenGiroId = this.solicitudPago.ordenGiro.ordenGiroId;

            if (this.solicitudPago.ordenGiro.ordenGiroDetalle !== undefined) {
                if (this.solicitudPago.ordenGiro.ordenGiroDetalle.length > 0) {
                    this.ordenGiroDetalle = this.solicitudPago.ordenGiro.ordenGiroDetalle[0];
                    this.ordenGiroDetalleId = this.ordenGiroDetalle.ordenGiroDetalleId;

                    if (this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion !== undefined) {
                        if (this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion.length > 0) {
                            this.ordenGiroDetalleTerceroCausacion = this.ordenGiroDetalle.ordenGiroDetalleTerceroCausacion;
                        }
                    }
                }
            }
        }

        /**
         * Se realiza una busqueda del balance financiero por el id de la orden de giro
         */
        if (this.balanceFinanciero.balanceFinancieroTraslado !== undefined) {
            if (this.balanceFinanciero.balanceFinancieroTraslado.length > 0) {
                const traslado = this.balanceFinanciero.balanceFinancieroTraslado.find(traslado => traslado.ordenGiroId === this.ordenGiroId)

                if (traslado !== undefined) {
                    this.balanceFinancieroTraslado = traslado;
                    this.balanceFinancieroTrasladoId = this.balanceFinancieroTraslado.balanceFinancieroTrasladoId;
                    this.balanceFinancieroTrasladoValor = this.balanceFinancieroTraslado.balanceFinancieroTrasladoValor;
                }
            }
        }

        if (this.balanceFinanciero) {
            this.balanceFinancieroId = this.balanceFinanciero.balanceFinancieroId;
        }

        // Get Tablas
        this.solicitudPagoFaseCriterio = this.solicitudPagoFase.solicitudPagoFaseCriterio;
        this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFase.solicitudPagoFaseFacturaDescuento;

        if (this.solicitudPago.contratoSon.solicitudPago.length > 1) {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.contratoSon.solicitudPago[0].solicitudPagoCargarFormaPago[0];
        } else {
            this.fasePreConstruccionFormaPagoCodigo = this.solicitudPago.solicitudPagoCargarFormaPago[0];
        }
        // Get data valor neto giro
        this.solicitudPagoFaseCriterio.forEach(criterio => this.valorNetoGiro += criterio.valorFacturado);
        if (this.solicitudPagoFaseFacturaDescuento.length > 0) {
            this.solicitudPagoFaseFacturaDescuento.forEach(descuento => this.valorNetoGiro -= descuento.valorDescuento);
        }
        //this.solicitudPago.ordenGiro.ordenGiroDetalle[0].ordenGiroDetalleTerceroCausacion[0].ordenGiroDetalleTerceroCausacionDescuento
        this.solicitudPago.ordenGiro.ordenGiroDetalle.forEach(ordengiro => {
            ordengiro.ordenGiroDetalleTerceroCausacion.filter(r => r.contratacionProyectoId == this.contratacionProyectoId).forEach(tercero => {
                tercero.ordenGiroDetalleTerceroCausacionDescuento.forEach(descuento => {
                    this.valorNetoGiro -= descuento.valorDescuento
                });
            });
        });
        /*
            get listaCriterios para lista desplegable
            Se reutilizan los servicios del CU 4.1.7 "Solicitud de pago"
        */
        this.registrarPagosSvc.getCriterioByFormaPagoCodigo(
            this.solicitudPagoFase.esPreconstruccion === true ? this.fasePreConstruccionFormaPagoCodigo.fasePreConstruccionFormaPagoCodigo : this.fasePreConstruccionFormaPagoCodigo.faseConstruccionFormaPagoCodigo
        )
            .subscribe(
                async getCriterioByFormaPagoCodigo => {
                    // Get constantes y variables
                    const listCriterios = [];
                    // Busqueda de criterios seleccionados en el CU 4.1.7 en la lista de tipo dominio
                    for (const criterioValue of this.solicitudPagoFaseCriterio) {
                        const criterioFind = getCriterioByFormaPagoCodigo.find(value => value.codigo === criterioValue.tipoCriterioCodigo);
                        const listConceptos = [];
                        if (criterioFind !== undefined) {
                            // Get lista dominio de los tipos de pago por criterio codigo
                            const tiposDePago = await this.registrarPagosSvc.getTipoPagoByCriterioCodigo(criterioFind.codigo);
                            const tipoPago = tiposDePago.find(tipoPago => tipoPago.codigo === criterioValue.tipoPagoCodigo);
                            // Get lista dominio de los conceptos de pago por tipo de pago codigo
                            const conceptosDePago = await this.registrarPagosSvc.getConceptoPagoCriterioCodigoByTipoPagoCodigo(tipoPago.codigo);

                            // Get data de los conceptos diligenciados en el CU 4.1.7
                            for (const conceptoValue of criterioValue.solicitudPagoFaseCriterioConceptoPago) {
                                const conceptoFind = conceptosDePago.find(value => value.codigo === conceptoValue.conceptoPagoCriterio);
                                if (conceptoFind !== undefined) {
                                    listConceptos.push({ ...conceptoFind, valorFacturadoConcepto: conceptoValue.valorFacturadoConcepto, usoCodigo: conceptoValue?.usoCodigo });
                                }
                            }
                            listCriterios.push(
                                {
                                    tipoCriterioCodigo: criterioFind.codigo,
                                    nombre: criterioFind.nombre,
                                    tipoPagoCodigo: tipoPago.codigo,
                                    listConceptos
                                }
                            );
                        }
                    }

                    console.log(listCriterios)
                    //const dataAportantes = await this.ordenGiroSvc.getAportantes( this.solicitudPago );
                    const dataAportantes = await this.ordenGiroSvc.getAportantesNew(this.solicitudPago)

                    if (this.solicitudPago.tablaUsoFuenteAportante !== undefined) {
                        if (this.solicitudPago.tablaUsoFuenteAportante.usos !== undefined) {
                            if (this.solicitudPago.tablaUsoFuenteAportante.usos.length > 0) {
                                this.solicitudPago.tablaUsoFuenteAportante.usos.forEach(uso => {
                                    if (uso.fuentes !== undefined) {
                                        if (uso.fuentes.length > 0) {
                                            uso.fuentes.forEach(fuente => {
                                                if (fuente.aportante !== undefined) {
                                                    if (fuente.aportante.length > 0) {
                                                        fuente.aportante.forEach(aportante => {
                                                            dataAportantes.listaNombreAportante.find(nombreAportante => {
                                                                if (nombreAportante.cofinanciacionAportanteId === aportante.aportanteId) {
                                                                    nombreAportante.valorActual = Number(aportante.valor.split('.').join(''))
                                                                }
                                                            })
                                                        })
                                                    }
                                                }
                                            })
                                        }
                                    }
                                })
                            }
                        }
                    }

                    // Get cantidad de aportantes para limitar cuantos aportantes se pueden agregar en el formulario
                    this.cantidadAportantes = dataAportantes.listaTipoAportante.length;
                    // Get data del guardado de tercero de causacion
                    for (const criterio of listCriterios) {
                        let totalCompleto = 0;
                        let totalIncompleto = 0;
                        let terceroCausacionxCriterio;
                        if (this.ordenGiroDetalleTerceroCausacion != null) {
                            terceroCausacionxCriterio = this.ordenGiroDetalleTerceroCausacion.filter(tercero => tercero.conceptoPagoCriterio === criterio.tipoCriterioCodigo && tercero.esPreconstruccion === this.esPreconstruccion && tercero.contratacionProyectoId === this.solicitudPagoFase.contratacionProyectoId);
                        }
                        /**
                         * Variables para validar el semaforo
                         */
                        let semaforoCriterio = 'sin-diligenciar';
                        let cantidadRegistroCompletoAportante = 0;
                        let cantidadRegistroCompletoDescuento = 0;
                        let cantidadDescuentosAportantes = 0;
                        /**
                         * Lista de FormGroup que se inyectara en el campo "conceptos" en el formulario del criterio
                         */
                        const conceptosDePago: FormGroup[] = [];
                        /**
                         * Se realiza la iteracion de los conceptos por criterio
                         */
                        for (const concepto of criterio.listConceptos) {
                            // debo crear un ordenGiroDetalleTerceroCausacion x conceptoCriterio
                            let terceroCausacion;
                            if (terceroCausacionxCriterio != null) {
                                terceroCausacion = terceroCausacionxCriterio.find(r => r.conceptoCodigo == concepto.codigo);
                            }
                            const listDescuento = [...this.tipoDescuentoArray];
                            const listaDescuentos = [];
                            const listaAportanteDescuentos = [];
                            //el ordenGiroDetalleTerceroCausacion tiene por debajo aportantes y descuentos
                            /**Descuentos */
                            if (terceroCausacion != null) {
                                if (terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.length > 0) {
                                    for (const descuento of listDescuento) {
                                        const ordenGiroDetalleTerceroCausacionDescuento: any[] = terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.filter(ordenGiroDetalleTerceroCausacionDescuento => ordenGiroDetalleTerceroCausacionDescuento.tipoDescuentoCodigo === descuento.codigo);
                                        const listaAportanteDescuentos = [];

                                        if (ordenGiroDetalleTerceroCausacionDescuento.length > 0) {
                                            for (const terceroCausacionDescuento of ordenGiroDetalleTerceroCausacionDescuento) {
                                                const nombreAportante = dataAportantes.listaNombreAportante.find(nombre => nombre.cofinanciacionAportanteId === terceroCausacionDescuento.aportanteId);
                                                let listaFuenteRecursos: any[];
                                                let fuente: any;
                                                let valorTraslado: number;
                                                let balanceFinancieroTrasladoValorId = 0;

                                                if (nombreAportante !== undefined) {
                                                    listaFuenteRecursos = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId(nombreAportante.cofinanciacionAportanteId).toPromise();
                                                    fuente = listaFuenteRecursos.find(fuente => fuente.codigo === terceroCausacionDescuento.fuenteRecursosCodigo);
                                                }

                                                if (this.balanceFinancieroTrasladoValor !== undefined) {
                                                    if (this.balanceFinancieroTrasladoValor.length > 0) {
                                                        const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                                            tv =>
                                                                tv.ordenGiroDetalleTerceroCausacionDescuentoId === terceroCausacionDescuento.ordenGiroDetalleTerceroCausacionDescuentoId &&
                                                                tv.tipoTrasladoCodigo === String(this.tipoTrasladoCodigo.direccionFinanciera)
                                                        )

                                                        if (balanceFinancieroTrasladoValor !== undefined) {
                                                            valorTraslado = balanceFinancieroTrasladoValor.valorTraslado;
                                                            balanceFinancieroTrasladoValorId = balanceFinancieroTrasladoValor.balanceFinancieroTrasladoValorId;
                                                        }
                                                    }
                                                }

                                                listaAportanteDescuentos.push(
                                                    this.fb.group(
                                                        {
                                                            registroCompleto: [ valorTraslado !== undefined ? (valorTraslado < 0 ? false : true) : false ],
                                                            ordenGiroDetalleTerceroCausacionId: [{ value: terceroCausacion.ordenGiroDetalleTerceroCausacionId, disabled: true }],
                                                            balanceFinancieroTrasladoValorId: [balanceFinancieroTrasladoValorId],
                                                            tipoTrasladoCodigo: [String(this.tipoTrasladoCodigo.direccionFinanciera)],
                                                            ordenGiroDetalleTerceroCausacionDescuentoId: [{ value: terceroCausacionDescuento.ordenGiroDetalleTerceroCausacionDescuentoId, disabled: true }],
                                                            nombreAportante: [{ value: nombreAportante !== undefined ? nombreAportante : null, disabled: true }, Validators.required],
                                                            valorDescuento: [{ value: terceroCausacionDescuento.valorDescuento, disabled: true }, Validators.required],
                                                            fuente: [{ value: fuente !== undefined ? fuente : null, disabled: true }, Validators.required],
                                                            nuevoValorDescuento: [
                                                                valorTraslado !== undefined ? (valorTraslado < 0 ? null : valorTraslado) : null,
                                                                Validators.required
                                                            ]
                                                        }
                                                    )
                                                )
                                            }

                                            if (listaAportanteDescuentos.length === 0) {
                                                listaAportanteDescuentos.push(
                                                    this.fb.group(
                                                        {
                                                            registroCompleto: [ false ],
                                                            ordenGiroDetalleTerceroCausacionId: [{ value: terceroCausacion.ordenGiroDetalleTerceroCausacionId, disabled: true }],
                                                            ordenGiroDetalleTerceroCausacionDescuentoId: [0],
                                                            nombreAportante: [{ value: null, disabled: true }, Validators.required],
                                                            valorDescuento: [{ value: null, disabled: true }, Validators.required],
                                                            fuente: [{ value: null, disabled: true }, Validators.required]
                                                        }
                                                    )
                                                )
                                            }

                                            listaDescuentos.push(
                                                this.fb.group(
                                                    {
                                                        tipoDescuento: [{ value: descuento.codigo, disabled: true }, Validators.required],
                                                        aportantesDescuento: this.fb.array(listaAportanteDescuentos)
                                                    }
                                                )
                                            );
                                        }
                                    }

                                    terceroCausacion.ordenGiroDetalleTerceroCausacionDescuento.forEach(descuento => {

                                        if (descuento.tipoDescuentoCodigo !== undefined) {
                                            const descuentoIndex = listDescuento.findIndex(descuentoIndex => descuentoIndex.codigo === descuento.tipoDescuentoCodigo);

                                            if (descuentoIndex !== -1) {
                                                listDescuento.splice(descuentoIndex, 1);
                                            }
                                        }
                                    });
                                }
                                /**
                                 * Lista de FormGroup que se inyectara en el campo "aportantes" en el formulario del concepto
                                 */
                                const listaAportantes: FormGroup[] = [];
                                /**
                                 * Se verifica que exista un registro de tercero de causacion con el concepto actual de la iteracion de conceptos
                                 * En caso de que no exista se realiza un push a la constante "listaAportantes" de un formulario vacio para que no se estalle el template del formulario
                                 */
                                if (terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.filter(r => r.conceptoPagoCodigo == concepto.codigo).length === 0) {
                                    listaAportantes.push(
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionId: [{ value: terceroCausacion.ordenGiroDetalleTerceroCausacionId, disabled: true }],
                                                ordenGiroDetalleTerceroCausacionAportanteId: [0],
                                                tipoAportante: [{ value: null, disabled: true }, Validators.required],
                                                listaNombreAportantes: [{ value: null, disabled: true }],
                                                nombreAportante: [{ value: null, disabled: true }, Validators.required],
                                                fuenteDeRecursos: [{ value: null, disabled: true }],
                                                fuenteRecursos: [{ value: null, disabled: true }, Validators.required],
                                                fuenteFinanciacionId: [{ value: null, disabled: true }],
                                                valorDescuento: [{ value: null, disabled: true }, Validators.required],
                                                nuevoValorDescuento: [null, Validators.required],
                                                valorDescuentoTecnica: [{ value: null, disabled: true }]
                                            }
                                        )
                                    )
                                }
                                /**
                                 * Se realiza la iteracion de los aportantes encontrados en el filtro de la tabla "ordenGiroDetalleTerceroCausacionAportante"
                                 * Con el concepto actual de la iteracion de conceptos
                                 */
                                for (const aportante of terceroCausacion.ordenGiroDetalleTerceroCausacionAportante.filter(r => r.conceptoPagoCodigo == concepto.codigo)) {
                                    const nombreAportante = dataAportantes.listaNombreAportante.find(nombre => nombre.cofinanciacionAportanteId === aportante.aportanteId);

                                    if (nombreAportante !== undefined) {
                                        const tipoAportante = dataAportantes.listaTipoAportante.find(tipo => tipo.dominioId === nombreAportante.tipoAportanteId);
                                        const tipoAportanteIndex = dataAportantes.listaTipoAportante.findIndex(tipo => tipo.dominioId === nombreAportante.tipoAportanteId);
                                        let listaFuenteRecursos: any[] = await this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId(nombreAportante.cofinanciacionAportanteId).toPromise();
                                        const fuente = listaFuenteRecursos.find(fuente => fuente.codigo === aportante.fuenteRecursoCodigo);
                                        let valorTraslado: number;
                                        let balanceFinancieroTrasladoValorId = 0;

                                        if (this.balanceFinancieroTrasladoValor !== undefined) {
                                            if (this.balanceFinancieroTrasladoValor.length > 0) {
                                                const balanceFinancieroTrasladoValor = this.balanceFinancieroTrasladoValor.find(
                                                    tv =>
                                                        tv.ordenGiroDetalleTerceroCausacionAportanteId === aportante.ordenGiroDetalleTerceroCausacionAportanteId &&
                                                        tv.tipoTrasladoCodigo === String(this.tipoTrasladoCodigo.aportante)
                                                )

                                                if (balanceFinancieroTrasladoValor !== undefined) {
                                                    valorTraslado = balanceFinancieroTrasladoValor.valorTraslado;
                                                    balanceFinancieroTrasladoValorId = balanceFinancieroTrasladoValor.balanceFinancieroTrasladoValorId;
                                                }
                                            }
                                        }

                                        listaAportanteDescuentos.push(nombreAportante)

                                        listaAportantes.push(
                                            this.fb.group(
                                                {
                                                    registroCompleto: [ valorTraslado !== undefined ? (valorTraslado < 0 ? false : true) : false ],
                                                    ordenGiroDetalleTerceroCausacionId: [{ value: terceroCausacion.ordenGiroDetalleTerceroCausacionId, disabled: true }],
                                                    balanceFinancieroTrasladoValorId: [balanceFinancieroTrasladoValorId],
                                                    tipoTrasladoCodigo: [String(this.tipoTrasladoCodigo.aportante)],
                                                    ordenGiroDetalleTerceroCausacionAportanteId: [{ value: aportante.ordenGiroDetalleTerceroCausacionAportanteId, disabled: true }],
                                                    tipoAportante: [{ value: tipoAportante, disabled: true }, Validators.required],
                                                    listaNombreAportantes: [[nombreAportante]],
                                                    nombreAportante: [{ value: nombreAportante, disabled: true }, Validators.required],
                                                    fuenteDeRecursos: [listaFuenteRecursos],
                                                    fuenteRecursos: [{ value: fuente, disabled: true }, Validators.required],
                                                    fuenteFinanciacionId: [{ value: fuente.fuenteFinanciacionId, disabled: true }],
                                                    valorDescuento: [{ value: aportante.valorDescuento, disabled: true }, Validators.required],
                                                    nuevoValorDescuento: [
                                                        valorTraslado !== undefined ? (valorTraslado < 0 ? null : valorTraslado) : null,
                                                        Validators.required
                                                    ],
                                                    valorDescuentoTecnica: [null]
                                                }
                                            )
                                        )
                                    } else {
                                        listaAportantes.push(
                                            this.fb.group(
                                                {
                                                    registroCompleto: [ false ],
                                                    ordenGiroDetalleTerceroCausacionId: [terceroCausacion.ordenGiroDetalleTerceroCausacionId],
                                                    ordenGiroDetalleTerceroCausacionAportanteId: [0],
                                                    tipoAportante: [{ value: null, disabled: true }, Validators.required],
                                                    listaNombreAportantes: [{ value: null, disabled: true }],
                                                    nombreAportante: [{ value: null, disabled: true }, Validators.required],
                                                    fuenteDeRecursos: [{ value: null, disabled: true }],
                                                    fuenteRecursos: [{ value: null, disabled: true }, Validators.required],
                                                    fuenteFinanciacionId: [{ value: null, disabled: true }],
                                                    valorDescuento: [{ value: null, disabled: true }, Validators.required],
                                                    valorDescuentoTecnica: [{ value: null, disabled: true }]
                                                }
                                            )
                                        )
                                    }
                                }
                                //
                                const usoByConcepto = await this.registrarPagosSvc.getUsoByConceptoPagoCriterioCodigo(concepto.codigo, this.solicitudPago.contratoId);
                                let valorTotalUso = 0;
                                if (usoByConcepto.length > 0) {
                                    usoByConcepto.forEach(uso => valorTotalUso += uso.valorUso);
                                }

                                listaAportantes.forEach(aportante => {
                                    if (aportante.get('registroCompleto').value === true) {
                                        cantidadRegistroCompletoAportante++;
                                    }
                                });
        
                                if (terceroCausacion.tieneDescuento === true) {
                                    listaDescuentos.forEach(descuento => {
                                        const descuentoAportantes = descuento.get('aportantesDescuento') as FormArray;
        
                                        descuentoAportantes.controls.forEach(descuentoControl => {
                                            if (descuentoControl.get('registroCompleto').value === true) {
                                                cantidadRegistroCompletoDescuento++;
                                            }
        
                                            cantidadDescuentosAportantes++;
                                        });
                                    });
                                }
        
                                if (
                                    cantidadRegistroCompletoAportante > 0 &&
                                    cantidadRegistroCompletoAportante === listaAportantes.length &&
                                    cantidadRegistroCompletoDescuento === 0 &&
                                    cantidadRegistroCompletoDescuento < cantidadDescuentosAportantes
                                ) {
                                    semaforoCriterio = 'en-proceso';
                                }
                                if (
                                    cantidadRegistroCompletoAportante > 0 &&
                                    cantidadRegistroCompletoAportante === listaAportantes.length &&
                                    cantidadRegistroCompletoDescuento > 0 &&
                                    cantidadRegistroCompletoDescuento < cantidadDescuentosAportantes
                                ) {
                                    semaforoCriterio = 'en-proceso';
                                }
                                if (
                                    cantidadRegistroCompletoAportante > 0 &&
                                    cantidadRegistroCompletoDescuento > 0 &&
                                    cantidadRegistroCompletoAportante < listaAportantes.length &&
                                    cantidadRegistroCompletoDescuento < cantidadDescuentosAportantes
                                ) {
                                    semaforoCriterio = 'en-proceso';
                                }
                                if (
                                    cantidadRegistroCompletoAportante === 0 &&
                                    cantidadRegistroCompletoDescuento > 0 &&
                                    cantidadRegistroCompletoDescuento < cantidadDescuentosAportantes
                                ) {
                                    semaforoCriterio = 'en-proceso';
                                }
                                if (
                                    cantidadRegistroCompletoAportante > 0 &&
                                    cantidadRegistroCompletoDescuento === 0 &&
                                    cantidadRegistroCompletoAportante < listaAportantes.length
                                ) {
                                    semaforoCriterio = 'en-proceso';
                                }
                                if (
                                    cantidadRegistroCompletoAportante > 0 &&
                                    cantidadRegistroCompletoDescuento > 0 &&
                                    cantidadRegistroCompletoAportante === listaAportantes.length &&
                                    cantidadRegistroCompletoDescuento === cantidadDescuentosAportantes
                                ) {
                                    semaforoCriterio = 'completo';
                                }
                                if (
                                    cantidadRegistroCompletoAportante > 0 &&
                                    cantidadRegistroCompletoAportante === listaAportantes.length &&
                                    cantidadRegistroCompletoDescuento === 0 &&
                                    cantidadRegistroCompletoDescuento === cantidadDescuentosAportantes
                                ) {
                                    semaforoCriterio = 'completo';
                                }

                                conceptosDePago.push(this.fb.group(
                                    {
                                        ordenGiroDetalleTerceroCausacionId: [terceroCausacion.ordenGiroDetalleTerceroCausacionId],
                                        conceptoPagoCriterio: [{ value: concepto.codigo, disabled: true }],
                                        nombre: [{ value: concepto.nombre, disabled: true }],
                                        valorTotalUso: [{ value: valorTotalUso, disabled: true }],
                                        usoCodigo: [concepto?.usoCodigo ?? usoByConcepto[0]?.tipoUsoCodigo],
                                        valorFacturadoConcepto: [{ value: concepto.valorFacturadoConcepto, disabled: true }],
                                        tipoDeAportantes: [{ value: dataAportantes.listaTipoAportante, disabled: true }],
                                        nombreDeAportantes: [dataAportantes.listaNombreAportante],
                                        tipoDescuentoArray: [listDescuento],
                                        descuento: this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionId: [{ value: terceroCausacion.ordenGiroDetalleTerceroCausacionId, disabled: true }],
                                                aplicaDescuentos: [{ value: terceroCausacion.tieneDescuento, disabled: true }, Validators.required],
                                                numeroDescuentos: [{ value: listaDescuentos.length > 0 ? listaDescuentos.length : null, disabled: true }, Validators.required],
                                                listaAportanteDescuentos: [listaAportanteDescuentos, Validators.required],
                                                descuentos: this.fb.array(listaDescuentos)
                                            }
                                        ),
                                        aportantes: this.fb.array(listaAportantes)
                                    }
                                ))
                            }
                        }

                        if (terceroCausacionxCriterio?.length > 0) {
                            this.criterios.push(this.fb.group(
                                {
                                    estadoSemaforo: [ semaforoCriterio ],
                                    tipoCriterioCodigo: [criterio.tipoCriterioCodigo],
                                    nombre: [criterio.nombre],
                                    tipoPagoCodigo: [criterio.tipoPagoCodigo],
                                    conceptos: this.fb.array(conceptosDePago.length > 0 ? conceptosDePago : [])
                                }
                            ))
                        } else {
                            this.criterios.push(this.fb.group(
                                {
                                    estadoSemaforo: ['sin-diligenciar'],
                                    tipoCriterioCodigo: [criterio.tipoCriterioCodigo],
                                    nombre: [criterio.nombre],
                                    tipoPagoCodigo: [criterio.tipoPagoCodigo],
                                    conceptos: this.fb.array(conceptosDePago.length > 0 ? conceptosDePago : [])
                                }
                            ))
                        }
                    }
                    const totalRegistrosCompletos = this.criterios.controls.filter(control => control.get('estadoSemaforo').value === 'completo').length
                    const totalRegistrosEnProceso = this.criterios.controls.filter(control => control.get('estadoSemaforo').value === 'en-proceso').length

                    if (totalRegistrosCompletos > 0 && totalRegistrosCompletos === this.criterios.length) {
                        this.estadoSemaforo.emit('completo')
                    }

                    if (totalRegistrosCompletos > 0 && totalRegistrosCompletos < this.criterios.length) {
                        this.estadoSemaforo.emit('en-proceso')
                    }

                    if (totalRegistrosEnProceso > 0 && totalRegistrosEnProceso < this.criterios.length) {
                        this.estadoSemaforo.emit('en-proceso')
                    }

                    if (totalRegistrosEnProceso > 0 && totalRegistrosEnProceso === this.criterios.length) {
                        this.estadoSemaforo.emit('en-proceso')
                    }
                }
            );
    }

    firstLetterUpperCase(texto: string) {
        if (texto !== undefined) {
            return humanize.capitalize(String(texto).toLowerCase());
        }
    }

    getDescuento(codigo: string) {
        if (this.tipoDescuentoArray.length > 0) {
            const descuento = this.tipoDescuentoArray.find(descuento => descuento.codigo === codigo);

            if (descuento !== undefined) {
                return descuento.nombre;
            }
        }
    }

    getTipoDescuento(codigo: string): Dominio[] {
        if (this.listaTipoDescuento.length > 0) {

            /*const descuento = this.listaTipoDescuento.find( descuento => descuento.codigo === codigo );

            if ( descuento !== undefined ) {
                return [ descuento ];
            }*/
            return this.listaTipoDescuento;
        }
    }

    checkValueAportante(
        value: number,
        index: number,
        jIndex: number,
        kIndex: number,
        tipoValor: string) {
        if (value !== null) {
            if (value < 0) {
                if (tipoValor === 'aportante') {
                    this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null);
                }
            }
        }
    }

    validateMaxSaldoActualAportante(value: number, index: number, jIndex: number, kIndex: number, aportante: any, fuenteFinanciacionId: any) {
        if (value !== null) {
            console.log(this.solicitudPago.tablaInformacionFuenteRecursos)
            // console.log(aportante.cofinanciacionAportanteId)

            if (this.solicitudPago.tablaInformacionFuenteRecursos) {
                const saldoAport = this.solicitudPago.tablaInformacionFuenteRecursos.find(obs => {
                    if (obs.cofinanciacionAportanteId === aportante.cofinanciacionAportanteId && obs.fuenteFinanciacionId == fuenteFinanciacionId) {
                        return obs.saldoActual
                    }
                })
                if (
                    value > saldoAport
                ) {
                    this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null);
                    this.openDialog('', `<b>El valor facturado por el concepto para el aportante no puede ser mayor al saldo que tiene el aportante para el uso.</b>`)
                    return
                }
            }
        }
    }

    validateDiscountAportanteValue(value: number, index: number, jIndex: number, kIndex: number) {
        if (value !== null) {
            if (value < 0) {
                this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null)
                return
            }
            let totalValueAportante = 0;
            this.getAportantes(index, jIndex).controls.forEach(aportanteControl => {
                if (aportanteControl.get('nuevoValorDescuento').value !== null) {
                    totalValueAportante += aportanteControl.get('nuevoValorDescuento').value;
                }
            })
            if (
                value > this.getValorAportante(this.getConceptos(index).controls[jIndex].get('conceptoPagoCriterio').value, this.getAportantes(index, jIndex).controls[kIndex].get('nombreAportante').value.cofinanciacionAportanteId, this.getAportantes(index, jIndex).controls[kIndex].get('fuenteRecursos').value?.fuenteFinanciacionId, this.getConceptos(index).controls[jIndex].get('usoCodigo').value)
            ) {
                this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null);
                this.openDialog('', `<b>El valor facturado por el concepto para el aportante no puede ser mayor al valor aportante para el concepto.</b>`)
                return
            }
            if (this.getAportantes(index, jIndex).controls[kIndex].get('nombreAportante').value !== null) {
                if (
                    value > this.getConceptos(index).controls[jIndex].get('valorTotalUso').value
                ) {
                    this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null);
                    this.openDialog('', `<b>El valor facturado por el concepto para el aportante no puede ser mayor al valor asignado por DRP al aportante.</b>`)
                    return
                }
            }

            if (totalValueAportante > this.getConceptos(index).controls[jIndex].get('valorTotalUso').value) {
                this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null);
                this.openDialog('', `<b>La suma total del valor facturado por el concepto para el aportante no puede ser mayor al valor del uso asociado al concepto.</b>`)
                return
            }


            let ordenGiroDetalleDescuentoTecnica = [];
            const ordenGiroDetalleDescuentoTecnicaAportante = [];
            let totalDescuentoAportante = 0;
            if (this.ordenGiroDetalle !== undefined) {
                if (this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica !== undefined) {
                    ordenGiroDetalleDescuentoTecnica = this.ordenGiroDetalle.ordenGiroDetalleDescuentoTecnica.filter(ordenGiroDetalleDescuentoTecnica => ordenGiroDetalleDescuentoTecnica.esPreconstruccion === this.esPreconstruccion);
                }
            }

            if (ordenGiroDetalleDescuentoTecnica.length > 0) {
                for (const descuentoTecnica of ordenGiroDetalleDescuentoTecnica) {
                    if (descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante !== undefined) {
                        if (descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante.length > 0) {
                            if (this.getAportantes(index, jIndex).controls[kIndex].get('nombreAportante').value !== null) {
                                const aportante = descuentoTecnica.ordenGiroDetalleDescuentoTecnicaAportante.find(
                                    descuentoTecnicaAportante => descuentoTecnicaAportante.conceptoPagoCodigo === this.getConceptos(index).controls[jIndex].get('conceptoPagoCriterio').value && descuentoTecnicaAportante.aportanteId === this.getAportantes(index, jIndex).controls[kIndex].get('nombreAportante').value.cofinanciacionAportanteId
                                )

                                if (aportante !== undefined) {
                                    ordenGiroDetalleDescuentoTecnicaAportante.push(aportante);
                                }
                            }
                        }
                    }
                }
            }

            if (ordenGiroDetalleDescuentoTecnicaAportante.length > 0) {
                ordenGiroDetalleDescuentoTecnicaAportante.forEach(descuentoTecnica => totalDescuentoAportante += descuentoTecnica.valorDescuento);
            }

            this.getAportantes(index, jIndex).controls[kIndex].get('valorDescuentoTecnica').setValue(totalDescuentoAportante);
            if (totalValueAportante > this.getConceptos(index).controls[jIndex].get('valorFacturadoConcepto').value) {
                this.getAportantes(index, jIndex).controls[kIndex].get('nuevoValorDescuento').setValue(null);
                this.openDialog('', `<b>La suma total del valor facturado por el concepto para el aportante no puede ser mayor al valor facturado por concepto o al valor aportante para el concepto.</b>`)
            }
        }
    }

    getValorAportante(codigo: string, aportanteId: any, fuenteFinanciacionId: any, usoCodigo: string) {
        let usoDelConcepto = usoCodigo != null && usoCodigo != undefined ? usoCodigo : this.solicitudPago.vConceptosUsosXsolicitudPagoId.find(r => r.conceptoCodigo == codigo && r.contratacionProyectoId == this.contratacionProyectoId)?.usoCodigo;
        let valorUsoTotal = 0;
        let valorFacturadoTotal = 0;
        if (aportanteId > 0 && codigo != null && fuenteFinanciacionId > 0 && usoDelConcepto != undefined && usoDelConcepto != null) {
            let drpData = this.usosDrp?.find(r => r.contratacionProyectoId == this.contratacionProyectoId && r.cofinanciacionAportanteId == aportanteId && r.fuenteFinanciacionId == fuenteFinanciacionId && r.esPreConstruccion == this.esPreconstruccion && r.tipoUsoCodigo == usoDelConcepto);
            let valoresFacturados = this.usosFacturados?.filter(r => r.contratacionProyectoId == this.contratacionProyectoId && r.aportanteId == aportanteId && r.fuenteFinanciacionId == fuenteFinanciacionId && r.usoCodigo == usoDelConcepto && r.esPreconstruccion == this.esPreconstruccion);
            if (drpData != null)
                valorUsoTotal = drpData?.valorUso ?? 0;

            if (valoresFacturados != null && valoresFacturados != null) {
                valoresFacturados.forEach(element => {
                    valorFacturadoTotal += element.valorDescuento ?? 0;
                });
            }
            if (valorUsoTotal > 0) return valorUsoTotal - valorFacturadoTotal;
        }
        return 0;
    }

    getCodigoDescuento(codigo: string, index: number, jIndex: number) {
        const listaDescuento: Dominio[] = this.getConceptos(index).controls[jIndex].get('tipoDescuentoArray').value;
        const descuentoIndex = listaDescuento.findIndex(descuento => descuento.codigo === codigo);

        /*if ( descuentoIndex !== -1 ) {
            //listaDescuento.splice( descuentoIndex, 1 );
        }*/
        this.getConceptos(index).controls[jIndex].get('tipoDescuentoArray').setValue(listaDescuento);
    }

    checkTotalValueAportantes(cb: { (totalValueAportantes: number): void }) {
        let totalAportantes = 0;

        this.criterios.controls.forEach((criterioControl, criterioIndex) => {

            this.getConceptos(criterioIndex).controls.forEach((conceptoControl, conceptoIndex) => {

                this.getAportantes(criterioIndex, conceptoIndex).controls.forEach(aportanteControl => totalAportantes += aportanteControl.get('valorDescuento').value);
            })
        })

        cb(totalAportantes);
    }

    // Check valor del descuento de los conceptos
    validateDiscountValue(value: number, index: number, jIndex: number, kIndex: number, lIndex: number) {
        let totalAportantePorConcepto = 0;

        if (value !== null) {
            if (value < 0) {
                this.getDescuentos(index, jIndex).controls[kIndex].get('valorDescuento').setValue(null)
                return
            }
        }

        for (const aportante of this.getConceptos(index).controls[jIndex].get('aportantes').value) {
            totalAportantePorConcepto += aportante.valorDescuento;
        }

        if (value > totalAportantePorConcepto) {
            this.getAportanteDescuentos(index, jIndex, kIndex).controls[lIndex].get('valorDescuento').setValue(null)
            this.openDialog('', `<b>El valor del descuento del concepto de pago no puede ser mayor al valor total de los aportantes.</b>`);
        }
    }

    getAportanteDescuento(aportante: any, index: number, jIndex: number, kIndex: number, lIndex: number) {

        const aportantes = this.getConceptos(index).controls[jIndex].get('aportantes') as FormArray;

        const aportanteControl = aportantes.controls.find(aportanteControl => aportanteControl.get('nombreAportante').value.tipoAportanteId === aportante.tipoAportanteId) as FormGroup;
        // console.log( aportantes )
        // console.log( aportante )
        // console.log( aportanteControl )

        if (aportanteControl !== undefined) {
            this.getAportanteDescuentos(index, jIndex, kIndex).controls[lIndex].get('fuente').setValue(aportanteControl.get('fuenteRecursos').value)
        }
    }

    validateNumberKeypress(event: KeyboardEvent) {
        const alphanumeric = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
        return alphanumeric.test(inputChar) ? true : false;
    }
    // Metodos para el formulario de addressForm
    valuePendingTipoAportante(aportanteSeleccionado: Dominio, index: number, jIndex: number, kIndex: number) {
        const listaAportantes: Dominio[] = this.getConceptos(index).controls[jIndex].get('tipoDeAportantes').value;
        const aportanteIndex = listaAportantes.findIndex(aportante => aportante.codigo === aportanteSeleccionado.codigo);
        const listaNombreAportantes: any[] = this.getConceptos(index).controls[jIndex].get('nombreDeAportantes').value;
        const filterAportantesDominioId = listaNombreAportantes.filter(aportante => aportante.tipoAportanteId === aportanteSeleccionado.dominioId);

        if (filterAportantesDominioId.length > 0) {
            this.getAportantes(index, jIndex).controls[kIndex].get('listaNombreAportantes').setValue(filterAportantesDominioId);
        }
    }

    getListaFuenteRecursos(nombreAportante: any, index: number, jIndex: number, kIndex: number) {
        const listaAportanteDescuentos: any[] = this.getConceptos(index).controls[jIndex].get('descuento').get('listaAportanteDescuentos').value;

        if (listaAportanteDescuentos.length > 0) {
            const aportante = listaAportanteDescuentos.find(aportante => aportante.tipoAportanteId === nombreAportante.tipoAportanteId);

            if (aportante === undefined) {
                listaAportanteDescuentos.push(nombreAportante);
            }
        } else {
            listaAportanteDescuentos.push(nombreAportante);
        }

        this.getConceptos(index).controls[jIndex].get('descuento').get('listaAportanteDescuentos').setValue(listaAportanteDescuentos)

        this.ordenGiroSvc.getFuentesDeRecursosPorAportanteId(nombreAportante.cofinanciacionAportanteId)
            .subscribe(fuenteRecursos => this.getAportantes(index, jIndex).controls[kIndex].get('fuenteDeRecursos').setValue(fuenteRecursos));
    }

    deleteAportanteDescuento(index: number, jIndex: number, kIndex: number, lIndex: number) {
        this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
            .subscribe(
                value => {
                    if (value === true) {
                        // deleteOrdenGiroDetalleTerceroCausacionDescuento
                        if (this.getAportanteDescuentos(index, jIndex, kIndex).controls[lIndex].get('ordenGiroDetalleTerceroCausacionDescuentoId').value !== 0) {
                            this.ordenGiroSvc.deleteOrdenGiroDetalleTerceroCausacionDescuento([this.getAportanteDescuentos(index, jIndex, kIndex).controls[lIndex].get('ordenGiroDetalleTerceroCausacionDescuentoId').value])
                                .subscribe(
                                    response => {
                                        this.getAportanteDescuentos(index, jIndex, kIndex).removeAt(lIndex);
                                        this.openDialog('', `<b>${response.message}</b>`);
                                    }
                                )
                        } else {
                            this.getAportanteDescuentos(index, jIndex, kIndex).removeAt(lIndex);
                            this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                        }
                    }
                }
            )
    }

    addAportanteDescuento(index: number, jIndex: number, kIndex: number, lIndex: number) {
        const listaAportanteDescuentos: any[] = this.getConceptos(index).controls[jIndex].get('descuento').get('listaAportanteDescuentos').value;

        if (this.getAportanteDescuentos(index, jIndex, kIndex).length < listaAportanteDescuentos.length) {
            this.getAportanteDescuentos(index, jIndex, kIndex).push(
                this.fb.group(
                    {
                        ordenGiroDetalleTerceroCausacionDescuentoId: [0],
                        nombreAportante: [null, Validators.required],
                        valorDescuento: [null, Validators.required],
                        fuente: [{ value: null, disabled: true }, Validators.required]
                    }
                )
            )
        }
    }

    deleteAportante(index: number, jIndex: number, kIndex: number) {
        this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
            .subscribe(
                value => {
                    if (value === true) {
                        const aportanteSeleccionado = this.getAportantes(index, jIndex).controls[kIndex].get('tipoAportante').value;

                        if (aportanteSeleccionado !== null) {
                            const listaTipoAportantes = this.getConceptos(index).controls[jIndex].get('tipoDeAportantes').value;
                            listaTipoAportantes.push(aportanteSeleccionado);

                            if (this.getAportantes(index, jIndex).controls[kIndex].get('ordenGiroDetalleTerceroCausacionAportanteId').value !== 0) {
                                this.ordenGiroSvc.deleteOrdenGiroDetalleTerceroCausacionAportante(this.getAportantes(index, jIndex).controls[kIndex].get('ordenGiroDetalleTerceroCausacionAportanteId').value)
                                    .subscribe(
                                        response => {
                                            this.getAportantes(index, jIndex).removeAt(kIndex);
                                            this.openDialog('', `${response.message}`);
                                        }
                                    )
                            } else {
                                this.getAportantes(index, jIndex).removeAt(kIndex);
                                this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                            }
                        } else {
                            this.getAportantes(index, jIndex).removeAt(kIndex);
                            this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                        }
                    }
                }
            )
    }

    addAportante(index: number, jIndex: number) {
        if (this.getAportantes(index, jIndex).length < this.cantidadAportantes) {
            this.getAportantes(index, jIndex).push(
                this.fb.group(
                    {
                        ordenGiroDetalleTerceroCausacionAportanteId: [0],
                        tipoAportante: [null, Validators.required],
                        listaNombreAportantes: [null],
                        nombreAportante: [null, Validators.required],
                        fuenteDeRecursos: [null],
                        fuenteRecursos: [null, Validators.required],
                        fuenteFinanciacionId: [null],
                        valorDescuento: [null, Validators.required],
                        valorDescuentoTecnica: [null]
                    }
                )
            )
        } else {
            this.openDialog('', '<b>El contrato no tiene más aportantes asignados.</b>')
        }
    }

    getCantidadDescuentos(value: number, index: number, jIndex: number) {
        if (value !== null && value > 0) {
            if (this.getConceptos(index).controls[jIndex].get('tipoDescuentoArray').value.length > 0) {
                if (this.getDescuentos(index, jIndex).dirty === true) {
                    const nuevosDescuentos = value - this.getDescuentos(index, jIndex).length;
                    this.getConceptos(index).controls[jIndex].get('descuento').get('numeroDescuentos').setValidators(Validators.min(this.getDescuentos(index, jIndex).length));

                    if (value < this.getDescuentos(index, jIndex).length) {
                        this.openDialog('', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>');
                        this.getConceptos(index).controls[jIndex].get('descuento').get('numeroDescuentos').setValue(this.getDescuentos(index, jIndex).length);
                        return;
                    }

                    for (let i = 0; i < nuevosDescuentos; i++) {

                        this.getDescuentos(index, jIndex).push(
                            this.fb.group(
                                {
                                    tipoDescuento: [null, Validators.required],
                                    aportantesDescuento: this.fb.array([
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionDescuentoId: [0],
                                                nombreAportante: [null, Validators.required],
                                                valorDescuento: [null, Validators.required],
                                                fuente: [{ value: null, disabled: true }, Validators.required]
                                            }
                                        )
                                    ])
                                }
                            )
                        )

                    }
                }
                if (this.getDescuentos(index, jIndex).dirty === false) {
                    const nuevosDescuentos = value - this.getDescuentos(index, jIndex).length;
                    this.getConceptos(index).controls[jIndex].get('descuento').get('numeroDescuentos').setValidators(Validators.min(this.getDescuentos(index, jIndex).length));

                    if (value < this.getDescuentos(index, jIndex).length) {
                        this.openDialog('', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>');
                        this.getConceptos(index).controls[jIndex].get('descuento').get('numeroDescuentos').setValue(this.getDescuentos(index, jIndex).length);
                        return;
                    }


                    for (let i = 0; i < nuevosDescuentos; i++) {

                        this.getDescuentos(index, jIndex).push(
                            this.fb.group(
                                {
                                    tipoDescuento: [null, Validators.required],
                                    aportantesDescuento: this.fb.array([
                                        this.fb.group(
                                            {
                                                ordenGiroDetalleTerceroCausacionDescuentoId: [0],
                                                nombreAportante: [null, Validators.required],
                                                valorDescuento: [null, Validators.required],
                                                fuente: [{ value: null, disabled: true }, Validators.required]
                                            }
                                        )
                                    ])
                                }
                            )
                        )

                    }
                }
            } else {
                this.openDialog('', '<b>No tiene parametrizados más descuentos para aplicar al pago.</b>');
            }
        }
    }

    deleteDescuento(index: number, jIndex: number, kIndex: number) {
        this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>')
            .subscribe(
                value => {
                    if (value === true) {
                        const codigo: string = this.getDescuentos(index, jIndex).controls[kIndex].get('tipoDescuento').value;

                        if (codigo !== null) {
                            const listaDescuento: Dominio[] = this.getConceptos(index).controls[jIndex].get('tipoDescuentoArray').value;
                            const descuento = this.listaTipoDescuento.find(descuento => descuento.codigo === codigo);

                            if (descuento !== undefined) {
                                listaDescuento.push(descuento);
                                this.getConceptos(index).controls[jIndex].get('tipoDescuentoArray').setValue(listaDescuento);
                            }
                        }

                        const listIdDescuento = [];

                        this.getAportanteDescuentos(index, jIndex, kIndex).controls.forEach(aportanteDescuento => {
                            if (aportanteDescuento.get('ordenGiroDetalleTerceroCausacionDescuentoId').value !== 0) {
                                listIdDescuento.push(aportanteDescuento.get('ordenGiroDetalleTerceroCausacionDescuentoId').value)
                            }
                        })

                        this.ordenGiroSvc.deleteOrdenGiroDetalleTerceroCausacionDescuento(listIdDescuento).subscribe()

                        this.getDescuentos(index, jIndex).removeAt(kIndex);
                        this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
                        this.getConceptos(index).controls[jIndex].get('descuento').get('numeroDescuentos').setValue(this.getDescuentos(index, jIndex).length);
                    }
                }
            )
    }

    openDialog(modalTitle: string, modalText: string) {
        const dialogRef = this.dialog.open(ModalDialogComponent, {
            width: '28em',
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

    disableSave(index: number) {
        const conceptos = this.getConceptos(index)?.value;
        if (conceptos != null) {
            if (conceptos.length > 0) {
                conceptos.forEach(concepto => {
                    if (concepto.aportantes != null) {
                        if (concepto.aportantes.length > 0) {
                            concepto.aportantes.forEach(aportante => {
                                if (aportante.nuevoValorDescuento == null || aportante.nuevoValorDescuento == undefined || aportante.nuevoValorDescuento <= 0) {
                                    return false;
                                }
                            });
                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                });
            } else {
                return false;
            }
        } else {
            return false;
        }
        return true;
    }

    guardar(index: number) {
        let alert = true
        let alert2 = false
        const conceptos = this.getConceptos(index)?.value;
        if (conceptos != null) {
            if (conceptos.length > 0) {
                conceptos.forEach(concepto => {
                    console.log( concepto )
                    if (concepto.aportantes != null) {
                        if (concepto.aportantes.length > 0) {
                            concepto.aportantes.forEach(aportante => {
                                if (aportante.nuevoValorDescuento == null || aportante.nuevoValorDescuento == undefined || aportante.nuevoValorDescuento <= 0) {
                                    alert = false;
                                }
                            });
                        } else {
                            alert = false;
                        }
                    } else {
                        alert = false;
                    }
                });
            } else {
                alert = false;
            }
        } else {
            alert = false;
        }

        this.getConceptos(index).controls.forEach( ( conceptoControl, conceptoIndex ) => {
            let totalAportantes = 0
            this.getAportantes( index, conceptoIndex ).controls.forEach( aportanteControl => {
                if (    aportanteControl.get( 'nuevoValorDescuento' ).value !== null
                        && typeof aportanteControl.get( 'nuevoValorDescuento' ).value === 'number'
                        && aportanteControl.get( 'nuevoValorDescuento' ).value >= 0 )
                {
                    totalAportantes = totalAportantes + aportanteControl.get( 'nuevoValorDescuento' ).value
                }
            } )

            if ( totalAportantes > conceptoControl.get( 'valorFacturadoConcepto' ).value ) alert2 = true
            if ( totalAportantes < conceptoControl.get( 'valorFacturadoConcepto' ).value ) alert2 = true

            if ( totalAportantes > conceptoControl.get( 'valorFacturadoConcepto' ).value || totalAportantes < conceptoControl.get( 'valorFacturadoConcepto' ).value ) {
                this.getAportantes( index, conceptoIndex ).controls.forEach( aportanteControl => aportanteControl.get( 'nuevoValorDescuento' ).markAsTouched() )
            }
        } )

        if ( alert2 ) {
            this.openDialog('', `<b>Los nuevos valores registrados no coinciden con el valor total de la facturación original., es necesario revisar y ajustar estos valores</b>`);
            return; 
        }

        if (!alert) {
            this.openDialog('', `<b>Debe ingresar todos los valores facturados por aportante</b>`);
            return;
        }

        const getBalanceFinancieroTrasladoValor = () => {
            const balanceFinancieroTraslado = [];

            this.criterios.controls.forEach((criterioControl, indexCriterio) => {
                if (criterioControl.dirty === true) {
                    this.getConceptos(indexCriterio).controls.forEach((conceptoControl, indexConcepto) => {
                        this.getAportantes(indexCriterio, indexConcepto).controls.forEach(aportanteControl => {
                            if (aportanteControl.get('nuevoValorDescuento').value) {
                                balanceFinancieroTraslado.push({
                                    balanceFinancieroTrasladoId: this.balanceFinancieroTrasladoId,
                                    esPreconstruccion: this.ordenGiroDetalleTerceroCausacion && this.ordenGiroDetalleTerceroCausacion.length > 0 ? this.ordenGiroDetalleTerceroCausacion[0].esPreconstruccion : false,
                                    balanceFinancieroTrasladoValorId: aportanteControl.get('balanceFinancieroTrasladoValorId').value,
                                    tipoTrasladoCodigo: aportanteControl.get('tipoTrasladoCodigo').value,
                                    ordenGiroDetalleTerceroCausacionAportanteId: aportanteControl.get('ordenGiroDetalleTerceroCausacionAportanteId').value,
                                    valorTraslado: aportanteControl.get('nuevoValorDescuento').value
                                });
                            }

                        });

                        this.getDescuentos(indexCriterio, indexConcepto).controls.forEach((descuentoControl, indexDescuento) => {
                            this.getAportanteDescuentos(indexCriterio, indexConcepto, indexDescuento).controls.forEach(
                                aportanteDescControl => {
                                    if (aportanteDescControl.get('nuevoValorDescuento').value) {
                                        balanceFinancieroTraslado.push({
                                            balanceFinancieroTrasladoId: this.balanceFinancieroTrasladoId,
                                            esPreconstruccion: this.ordenGiroDetalleTerceroCausacion && this.ordenGiroDetalleTerceroCausacion.length > 0 ? this.ordenGiroDetalleTerceroCausacion[0].esPreconstruccion : false,
                                            balanceFinancieroTrasladoValorId: aportanteDescControl.get('balanceFinancieroTrasladoValorId').value,
                                            tipoTrasladoCodigo: aportanteDescControl.get('tipoTrasladoCodigo').value,
                                            ordenGiroDetalleTerceroCausacionDescuentoId: aportanteDescControl.get('ordenGiroDetalleTerceroCausacionDescuentoId').value,
                                            valorTraslado: aportanteDescControl.get('nuevoValorDescuento').value
                                        });
                                    }
                                }
                            )
                        })
                    });
                }
            });

            return balanceFinancieroTraslado.length > 0 ? balanceFinancieroTraslado : null;
        };

        if (this.balanceFinanciero.balanceFinancieroTraslado !== undefined) {
            if (this.balanceFinanciero.balanceFinancieroTraslado.length > 0) {
                const trasladoIndex = this.balanceFinanciero.balanceFinancieroTraslado.findIndex(
                    traslado => traslado.ordenGiroId === this.ordenGiroId
                );

                if (trasladoIndex !== -1) {
                    this.balanceFinanciero.balanceFinancieroTraslado.splice(trasladoIndex, 1);
                }

                this.balanceFinanciero.balanceFinancieroTraslado.push({
                    ordenGiroId: this.ordenGiroId,
                    balanceFinancieroId: this.balanceFinancieroId,
                    balanceFinancieroTrasladoId: this.balanceFinancieroTrasladoId,
                    balanceFinancieroTrasladoValor: getBalanceFinancieroTrasladoValor()
                });
            } else {
                this.balanceFinanciero.balanceFinancieroTraslado = [
                    {
                        ordenGiroId: this.ordenGiroId,
                        balanceFinancieroId: this.balanceFinancieroId,
                        balanceFinancieroTrasladoId: this.balanceFinancieroTrasladoId,
                        balanceFinancieroTrasladoValor: getBalanceFinancieroTrasladoValor()
                    }
                ];
            }
        } else {
            this.balanceFinanciero.balanceFinancieroTraslado = [
                {
                    ordenGiroId: this.ordenGiroId,
                    balanceFinancieroId: this.balanceFinancieroId,
                    balanceFinancieroTrasladoId: this.balanceFinancieroTrasladoId,
                    balanceFinancieroTrasladoValor: getBalanceFinancieroTrasladoValor()
                }
            ];
        }

        console.log(this.balanceFinanciero)

        this._balanceSvc.createEditBalanceFinanciero(this.balanceFinanciero).subscribe(
            response => {
                this.openDialog('', `<b>${response.message}</b>`);

                // this.routes.navigateByUrl( '/', {skipLocationChange: true} ).then(
                //     () => this.routes.navigate(
                //         [
                //             '/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarTraslado', this.activatedRoute.snapshot.params.id, this.solicitudPago.ordenGiro.numeroSolicitud
                //         ]
                //     )
                // );
                // location.reload();
            },
            err => this.openDialog('', `<b>${err.message}</b>`)
        );
    }

}
