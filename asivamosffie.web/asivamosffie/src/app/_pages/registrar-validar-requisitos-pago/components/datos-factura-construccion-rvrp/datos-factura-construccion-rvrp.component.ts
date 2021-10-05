import { RegistrarRequisitosPagoService } from './../../../../core/_services/registrarRequisitosPago/registrar-requisitos-pago.service';
import { Router } from '@angular/router';
import { Component, Input, OnChanges, OnInit, EventEmitter, Output, SimpleChanges } from '@angular/core';
import { Validators, FormBuilder, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ObservacionesMultiplesCuService } from 'src/app/core/_services/observacionesMultiplesCu/observaciones-multiples-cu.service';
import { TiposDeFase } from 'src/app/_interfaces/solicitud-pago.interface';

@Component({
  selector: 'app-datos-factura-construccion-rvrp',
  templateUrl: './datos-factura-construccion-rvrp.component.html',
  styleUrls: ['./datos-factura-construccion-rvrp.component.scss']
})
export class DatosFacturaConstruccionRvrpComponent implements OnInit, OnChanges {
  @Input() solicitudPago: any;
  @Input() esVerDetalle = false;
  @Input() tieneObservacion: boolean;
  @Input() datosFacturaCodigo: string;
  @Input() listaMenusId: any;
  @Input() tieneObservacionOrdenGiro: boolean;
  @Input() faseCodigo: string;
  @Input() contratacionProyectoId: number;
  @Input() boolAplicaDescuentos: boolean;
  @Output() semaforoObservacion = new EventEmitter<boolean>();
  esPreconstruccion = true;
  addressForm = this.fb.group({
    tipoDocumento: [ null, Validators.required ],
    numeroFactura: [null, Validators.required],
    fechaFactura: [null, Validators.required],
    numeroDescuentos: [''],
    valorAPagarDespues: [{ value: null, disabled: true }],
    aplicaDescuento: [null],
    descuentos: this.fb.array([])
  });
  listaTipoDocumento: Dominio[] = [
    { nombre: 'Factura', codigo: '1' },
    { nombre: 'Cuenta de cobro', codigo: '2' }
  ];
  fasesContrato = TiposDeFase;
  valorFacturado = 0;
  tiposDescuentoArray: Dominio[] = [];
  listaTipoDescuento: Dominio[] = [];
  solicitudPagoFaseFacturaDescuento: any[] = [];
  solicitudPagoFaseFactura: any;
  solicitudPagoFase: any;
  esAutorizar: boolean;
  observacion: any;
  solicitudPagoObservacionId = 0;
  estaEditando = false;
  minDate = new Date();

  get descuentos() {
    return this.addressForm.get('descuentos') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private commonSvc: CommonService,
    private routes: Router,
    private obsMultipleSvc: ObservacionesMultiplesCuService,
    private registrarPagosSvc: RegistrarRequisitosPagoService
  ) {}

  ngOnInit(): void {
    this.getDatosFactura();
    if (this.boolAplicaDescuentos) this.addressForm.get('aplicaDescuento').setValue(false);
  }
  
  ngOnChanges(changes: SimpleChanges): void {
    if (changes.boolAplicaDescuentos)
      if (this.boolAplicaDescuentos) this.addressForm.get('aplicaDescuento').setValue(false);
      if (!this.boolAplicaDescuentos) this.addressForm.get('aplicaDescuento').setValue(null);
  }

  async getDatosFactura() {
    this.tiposDescuentoArray = await this.commonSvc.tiposDescuento().toPromise();
    this.listaTipoDescuento = [...this.tiposDescuentoArray];
    const solicitudPagoRegistrarSolicitudPago = this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0];

    // Verificar la fase seleccionada en el proyecto
    if ( this.faseCodigo === this.fasesContrato.construccion ) {
      this.esPreconstruccion = false;
    }

    if ( this.esPreconstruccion === true && solicitudPagoRegistrarSolicitudPago !== undefined ) {
      if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
        if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
          for (const solicitudPagoFase of solicitudPagoRegistrarSolicitudPago.solicitudPagoFase) {
            if (solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
              this.solicitudPagoFase = solicitudPagoFase;
            }
          }
        }
      }
    }

    if ( this.esPreconstruccion === false && solicitudPagoRegistrarSolicitudPago !== undefined ) {
      if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase !== undefined ) {
        if ( solicitudPagoRegistrarSolicitudPago.solicitudPagoFase.length > 0 ) {
          for (const solicitudPagoFase of solicitudPagoRegistrarSolicitudPago.solicitudPagoFase) {
            if (solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
              this.solicitudPagoFase = solicitudPagoFase;
            }
          }
        }
      }
    }

    if ( solicitudPagoRegistrarSolicitudPago !== undefined ) {
      if ( this.solicitudPagoFase !== undefined ) {
        if ( this.solicitudPagoFase.tieneDescuento !== undefined ) {
          this.addressForm.get( 'aplicaDescuento' ).setValue( this.solicitudPagoFase.tieneDescuento )
        }
        this.solicitudPagoFaseFacturaDescuento = this.solicitudPagoFase.solicitudPagoFaseFacturaDescuento;
        if (this.solicitudPagoFaseFacturaDescuento !== undefined) {
          this.estaEditando = true;
          this.addressForm.markAllAsTouched();

          for (const descuento of this.solicitudPagoFaseFacturaDescuento) {
            this.descuentos.markAllAsTouched();
            this.descuentos.push(
              this.fb.group({
                solicitudPagoFaseFacturaDescuentoId: [descuento.solicitudPagoFaseFacturaDescuentoId],
                solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                tipoDescuentoCodigo: [descuento.tipoDescuentoCodigo],
                valorDescuento: [descuento.valorDescuento]
              })
            );
          }
    
          if (this.esVerDetalle === false) {
            // Get observacion CU autorizar solicitud de pago 4.1.9
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId( 
              this.listaMenusId.autorizarSolicitudPagoId,
              this.solicitudPago.solicitudPagoId,
              this.solicitudPagoFase.solicitudPagoFaseId,
              this.datosFacturaCodigo
            ).subscribe( response => {
                const observacion = response.find(obs => obs.archivada === false);
                if (observacion !== undefined) {
                  this.esAutorizar = true;
                  this.observacion = observacion;
    
                  if (this.observacion.tieneObservacion === true) {
                    this.semaforoObservacion.emit(true);
                    this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                  }
                }
            } );
    
            // Get observacion CU verificar solicitud de pago 4.1.8
            this.obsMultipleSvc.getObservacionSolicitudPagoByMenuIdAndSolicitudPagoId(
              this.listaMenusId.aprobarSolicitudPagoId,
              this.solicitudPago.solicitudPagoId,
              this.solicitudPagoFase.solicitudPagoFaseId,
              this.datosFacturaCodigo
            ).subscribe( response => {
                const observacion = response.find(obs => obs.archivada === false);
                if (observacion !== undefined) {
                  this.esAutorizar = false;
                  this.observacion = observacion;
    
                  if (this.observacion.tieneObservacion === true) {
                    this.semaforoObservacion.emit(true);
                    this.solicitudPagoObservacionId = observacion.solicitudPagoObservacionId;
                  }
                }
            } );
          }
        }

        for (const criterio of this.solicitudPagoFase.solicitudPagoFaseCriterio) {
          this.valorFacturado += criterio.valorFacturado;
        }

        if ( this.solicitudPagoFaseFacturaDescuento !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0 ) {
          let totalDescuentos = 0;
          let valorDespuesDescuentos = 0;

          this.descuentos.controls.forEach(control => {
            if (control.value.valorDescuento !== null) {
              totalDescuentos += control.value.valorDescuento;
            }
          });

          valorDespuesDescuentos = this.valorFacturado - totalDescuentos;
          this.addressForm.get( 'numeroDescuentos' ).setValue( `${ this.solicitudPagoFaseFacturaDescuento.length }` )
          this.addressForm.get('valorAPagarDespues').setValue( valorDespuesDescuentos );
        }

        this.addressForm.get('numeroDescuentos').valueChanges.subscribe(value => {
          value = Number(value);
          if (value <= this.listaTipoDescuento.length) {
            if (this.solicitudPagoFaseFacturaDescuento !== undefined && this.solicitudPagoFaseFacturaDescuento.length > 0) {
              if (value > 0) {
                this.descuentos.clear();
                for (const descuento of this.solicitudPagoFaseFacturaDescuento) {
                  this.estaEditando = true;
                  this.descuentos.markAllAsTouched();
                  this.descuentos.push(
                    this.fb.group({
                      solicitudPagoFaseFacturaDescuentoId: [descuento.solicitudPagoFaseFacturaDescuentoId],
                      solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                      tipoDescuentoCodigo: [descuento.tipoDescuentoCodigo],
                      valorDescuento: [descuento.valorDescuento]
                    })
                  );
                }
    
                this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
                const nuevosDescuentos = value - this.descuentos.length;
                if (value < this.descuentos.length) {
                  this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                  this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
                  return;
                }
                for (let i = 0; i < nuevosDescuentos; i++) {
                  this.estaEditando = true;
                  this.descuentos.markAllAsTouched();
                  this.descuentos.push(
                    this.fb.group({
                      solicitudPagoFaseFacturaDescuentoId: [0],
                      solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                      tipoDescuentoCodigo: [null],
                      valorDescuento: [null]
                    })
                  );
                }
              }
            } else {
              if (value > 0) {
                if (this.descuentos.dirty === true) {
                  this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
                  const nuevosDescuentos = value - this.descuentos.length;
                  if (value < this.descuentos.length) {
                    this.openDialog( '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>' );
                    this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
                    return;
                  }
                  for (let i = 0; i < nuevosDescuentos; i++) {
                    this.estaEditando = true;
                    this.descuentos.markAllAsTouched();
                    this.descuentos.push(
                      this.fb.group({
                        solicitudPagoFaseFacturaDescuentoId: [0],
                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                        tipoDescuentoCodigo: [null],
                        valorDescuento: [null]
                      })
                    );
                  }
                }
                if (this.descuentos.dirty === false) {
                  this.descuentos.clear();
                  for (let i = 0; i < value; i++) {
                    this.estaEditando = true;
                    this.descuentos.markAllAsTouched();
                    this.descuentos.push(
                      this.fb.group({
                        solicitudPagoFaseFacturaDescuentoId: [0],
                        solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
                        tipoDescuentoCodigo: [null],
                        valorDescuento: [null]
                      })
                    );
                  }
                }
              }
            }
          } else {
            this.addressForm.get('numeroDescuentos').setValue(String(this.listaTipoDescuento.length));
            this.openDialog( '', `<b>Tiene parametrizados ${this.listaTipoDescuento.length} descuentos para aplicar al pago.</b>` );
          }
        });
      }
    }
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  validateNumber(value: any) {
    if (isNaN(Number(value)) === true) {
      this.addressForm.get('numeroDescuentos').setValue('');
    }
  }

  getTipoDescuentoSeleccionado(codigo: string): Dominio[] {
    if (this.listaTipoDescuento.length > 0) {
      const descuento = this.listaTipoDescuento.find(descuento => descuento.codigo === codigo);

      if (descuento !== undefined) {
        return [descuento];
      }
    }
  }

  getCodigoTipoDescuento(codigo: string) {
    const descuentoIndex = this.tiposDescuentoArray.findIndex(descuento => descuento.codigo === codigo);

    if (descuentoIndex !== -1) {
      this.tiposDescuentoArray.splice(descuentoIndex, 1);
    }
  }

  totalPagoDescuentos() {
    let totalDescuentos = 0;
    let valorDespuesDescuentos = 0;
    this.descuentos.controls.forEach(control => {
      if (control.value.valorDescuento !== null) {
        totalDescuentos += control.value.valorDescuento;
      }
    });
    if (totalDescuentos > 0) {
      if (totalDescuentos > this.valorFacturado) {
        this.openDialog('', '<b>El valor total de los descuentos es mayor al valor facturado.</b>');
        this.addressForm.get('valorAPagarDespues').setValue(null);
        return;
      } else {
        valorDespuesDescuentos = this.valorFacturado - totalDescuentos;
        this.addressForm.get('valorAPagarDespues').setValue(valorDespuesDescuentos);
        return;
      }
    } else {
      this.addressForm.get('valorAPagarDespues').setValue(null);
      return;
    }
  }

  addDescuento() {
    if (this.tiposDescuentoArray.length > 0) {
      this.descuentos.push(
        this.fb.group({
          solicitudPagoFaseFacturaDescuentoId: [0],
          solicitudPagoFaseId: [ this.solicitudPagoFase.solicitudPagoFaseId ],
          tipoDescuentoCodigo: [null],
          valorDescuento: [null]
        })
      );
      this.addressForm.get('numeroDescuentos').setValidators(Validators.min(this.descuentos.length));
      this.addressForm.get('numeroDescuentos').setValue(String(this.descuentos.length));
    } else {
      this.openDialog('', '<b>No tiene parametrizados más descuentos para aplicar al pago.</b>');
    }
  }

  deleteDescuento(index: number, descuentoId: number) {
    this.openDialogTrueFalse('', '<b>¿Está seguro de eliminar esta información?</b>').subscribe(response => {
      if (response === true) {
        if (descuentoId === 0) {
          const codigo: string = this.descuentos.controls[index].get('tipoDescuentoCodigo').value;
          const descuento = this.listaTipoDescuento.find(descuento => descuento.codigo === codigo);

          if (descuento !== undefined) {
            this.tiposDescuentoArray.push(descuento);
          }

          this.descuentos.removeAt(index);
          this.addressForm.get('numeroDescuentos').setValue(`${this.descuentos.length}`);
          this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
        } else {
          this.registrarPagosSvc.deleteSolicitudPagoFaseFacturaDescuento(descuentoId).subscribe(
            () => {
              this.openDialog('', '<b>La información se ha eliminado correctamente.</b>');
              this.routes.navigateByUrl('/', { skipLocationChange: true })
                .then(() => this.routes.navigate([ '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId ]) );
            },
            err => this.openDialog('', `<b>${err.message}</b>`)
          );
        }
      }
    });
  }

  getTipoDescuento(tipoDescuentoCodigo: string) {
    if (this.tiposDescuentoArray.length > 0) {
      const descuento = this.tiposDescuentoArray.filter(descuento => descuento.codigo === tipoDescuentoCodigo);
      return descuento[0].nombre;
    }
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

  onSubmit() {
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    this.descuentos.markAllAsTouched();

    const getSolicitudPagoFaseFacturaDescuento = () => {
      if (this.descuentos.length > 0) {
        if (this.addressForm.get('aplicaDescuento').value === true) {
          return this.descuentos.value;
        } else {
          return [];
        }
      } else {
        return [];
      }
    };

    if (this.esPreconstruccion === true) {
      if (this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0) {
        for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
          if (solicitudPagoFase.esPreconstruccion === true && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
            solicitudPagoFase.solicitudPagoFaseFacturaDescuento = getSolicitudPagoFaseFacturaDescuento()
            solicitudPagoFase.tieneDescuento = this.addressForm.get('aplicaDescuento').value
          }
        }
      }
    }

    if (this.esPreconstruccion === false) {
      if (this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase.length > 0) {
        for (const solicitudPagoFase of this.solicitudPago.solicitudPagoRegistrarSolicitudPago[0].solicitudPagoFase) {
          if (solicitudPagoFase.esPreconstruccion === false && solicitudPagoFase.contratacionProyectoId === this.contratacionProyectoId) {
            solicitudPagoFase.solicitudPagoFaseFacturaDescuento = getSolicitudPagoFaseFacturaDescuento()
            solicitudPagoFase.tieneDescuento = this.addressForm.get('aplicaDescuento').value
          }
        }
      }
    }

    this.registrarPagosSvc.createEditNewPayment(this.solicitudPago).subscribe(
      response => {
        this.openDialog('', `<b>${response.message}</b>`);
        if (this.observacion !== undefined) {
          this.observacion.archivada = !this.observacion.archivada;
          this.obsMultipleSvc.createUpdateSolicitudPagoObservacion(this.observacion).subscribe();
        }
        this.routes.navigateByUrl('/', { skipLocationChange: true })
          .then( () => this.routes.navigate([ '/registrarValidarRequisitosPago/verDetalleEditar', this.solicitudPago.contratoId, this.solicitudPago.solicitudPagoId ]) );
      },
      err => this.openDialog('', `<b>${err.message}</b>`)
    );
  }
}
