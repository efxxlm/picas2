import { Component, Input, OnInit, Output, EventEmitter, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup } from '@angular/forms';
import { ContratacionProyecto2, ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { CommonService, Dominio } from '../../../../core/_services/common/common.service';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-form-perfil',
  templateUrl: './form-perfil.component.html',
  styleUrls: ['./form-perfil.component.scss']
})
export class FormPerfilComponent implements OnInit {

  formContratista: FormGroup;
  minDate: Date;
  @Input() perfilProyecto: any[] = [];
  @Input() contratoId: number;
  @Input() proyectoId: number;
  @Output() enviarPerfilesContrato = new EventEmitter();
  @Output() perfilesCompletados = new EventEmitter();
  @ViewChild( 'cantidadPerfiles', { static: true } ) cantidadPerfiles: ElementRef;
  perfilesCompletos = 0;
  perfilesEnProceso = 0;
  editorStyle = {
    height: '45px'
  };
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  perfilesCv: Dominio[] = [];

  get perfiles() {
    return this.formContratista.get( 'perfiles' ) as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private commonSvc: CommonService,
    private dialog: MatDialog,
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService )
  {
    this.minDate = new Date();
    this.crearFormulario();
    this.commonSvc.listaPerfil()
      .subscribe( perfiles => {
        this.perfilesCv = perfiles;
      } );
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.perfilesProyecto();
    }, 1000);
  }

  crearFormulario() {
    this.formContratista = this.fb.group({
      numeroPerfiles: [ '' ],
      perfiles: this.fb.array([])
    });
  }

  perfilesProyecto() {
    let observacionSupervisorSemaforo = null;
    if ( this.perfilProyecto.length === 0 ) {
      this.formContratista.get( 'numeroPerfiles' ).valueChanges
      .subscribe( value => {
        if ( this.formContratista.get( 'perfiles' ).dirty === true && Number( value ) > 0 ) {
          this.formContratista.get( 'numeroPerfiles' ).setValidators( Validators.min( this.perfiles.length ) );
          const nuevosPerfiles = Number( value ) - this.perfiles.length;
          if ( value < this.perfiles.length ) {
            this.openDialog(
              '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
            );
            this.formContratista.get( 'numeroPerfiles' ).setValue( String( this.perfiles.length ) );
            return;
          }
          for ( let i = 0; i < nuevosPerfiles; i++ ) {
            this.perfiles.push(
              this.fb.group(
                {
                  estadoSemaforo              : [ 'sin-diligenciar' ],
                  contratoPerfilId            : [ 0 ],
                  perfilCodigo                : [ null ],
                  cantidadHvRequeridas        : [ '' ],
                  cantidadHvRecibidas         : [ '' ],
                  cantidadHvAprobadas         : [ '' ],
                  fechaAprobacion             : [ null ],
                  observacion                 : [ null ],
                  observacionSupervisor       : [ null ],
                  fechaObservacion            : [ null ],
                  contratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                  rutaSoporte                 : [ '' ]
                }
              )
            );
          }
        }
        if ( this.formContratista.get( 'perfiles' ).dirty === false && Number( value ) > 0 ) {
          this.perfiles.clear();
          for ( let i = 0; i < Number(value); i++ ) {
            this.perfiles.push(
              this.fb.group(
                {
                  estadoSemaforo              : [ 'sin-diligenciar' ],
                  contratoPerfilId            : [ 0 ],
                  perfilCodigo                : [ null ],
                  cantidadHvRequeridas        : [ '' ],
                  cantidadHvRecibidas         : [ '' ],
                  cantidadHvAprobadas         : [ '' ],
                  fechaAprobacion             : [ null ],
                  observacion                 : [ null ],
                  observacionSupervisor       : [ null ],
                  fechaObservacion            : [ null ],
                  contratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                  rutaSoporte                 : [ '' ]
                }
              )
            );
          }
        }
      } );
      this.perfilesCompletados.emit( 'sin-diligenciar' );
    } else {
      this.formContratista.get( 'numeroPerfiles' ).setValue( String( this.perfilProyecto.length ) );
      this.formContratista.get( 'numeroPerfiles' ).setValidators( Validators.min( this.perfiles.length ) );
      this.formContratista.get( 'numeroPerfiles' ).valueChanges
        .subscribe(
          value => {
            if ( value < this.perfilProyecto.length && value > 0 ) {
              this.openDialog(
                '', '<b>Debe eliminar uno de los registros diligenciados para disminuir el total de los registros requeridos.</b>'
              );
              this.formContratista.get( 'numeroPerfiles' ).setValue( String( this.perfilProyecto.length ) );
              return;
            }
            const nuevosPerfiles = Number( value ) - this.perfiles.length;
            for ( let i = 0; i < nuevosPerfiles; i++ ) {
              this.perfiles.push(
                this.fb.group(
                  {
                    estadoSemaforo              : [ 'sin-diligenciar' ],
                    contratoPerfilId            : [ 0 ],
                    perfilCodigo                : [ null ],
                    cantidadHvRequeridas        : [ '' ],
                    cantidadHvRecibidas         : [ '' ],
                    cantidadHvAprobadas         : [ '' ],
                    fechaAprobacion             : [ null ],
                    observacion                 : [ null ],
                    observacionSupervisor       : [ null ],
                    fechaObservacion            : [ null ],
                    contratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                    rutaSoporte                 : [ '' ]
                  }
                )
              );
            }
          }
        );
      for ( const perfil of this.perfilProyecto ) {
        const observacionTipo3 = [];
        const numeroRadicados = [];
        let observaciones = null;
        let fechaObservacion = null;
        let observacionSupervisor = null;
        let semaforo;
        observacionSupervisorSemaforo = null;
        if ( perfil.contratoPerfilNumeroRadicado.length === 0 ) {
          numeroRadicados.push(
            this.fb.group(
              {
                contratoPerfilNumeroRadicadoId: 0,
                contratoPerfilId: perfil.contratoPerfilId,
                numeroRadicado: ''
              }
            )
          );
        } else {
          for ( const radicado of perfil.contratoPerfilNumeroRadicado ) {
            numeroRadicados.push(
              this.fb.group(
                { contratoPerfilNumeroRadicadoId: radicado.contratoPerfilNumeroRadicadoId || 0,
                  contratoPerfilId: perfil.contratoPerfilId,
                  numeroRadicado: radicado.numeroRadicado
                }
              )
            );
          }
        }

        if ( perfil.contratoPerfilObservacion.length > 0 ) {
          for ( const obs of perfil.contratoPerfilObservacion ) {
            if ( obs.tipoObservacionCodigo === '1' ) { observaciones = obs.observacion; }
            if ( obs.tipoObservacionCodigo === '3' && perfil.tieneObservacionSupervisor === true ) { observacionTipo3.push( obs ); }
          }
        }
        if ( observacionTipo3.length > 0 ) {
          fechaObservacion = observacionTipo3[ observacionTipo3.length - 1 ].fechaCreacion;
          observacionSupervisor = observacionTipo3[ observacionTipo3.length - 1 ].observacion;
          observacionSupervisorSemaforo = true;
        }
        if ( perfil.registroCompleto === true ) {
          this.perfilesCompletos++;
          semaforo = 'completo';
        }
        if (  !perfil.registroCompleto && (perfil.cantidadHvRequeridas > 0
              || perfil.cantidadHvRecibidas > 0 || perfil.cantidadHvAprobadas > 0) ) {
          semaforo = 'en-proceso';
          this.perfilesEnProceso++;
        }
        this.perfiles.push(
          this.fb.group(
            {
              estadoSemaforo              : [ observacionSupervisorSemaforo === true ? 'en-proceso' : ( semaforo ? semaforo : 'sin-diligenciar' ) ],
              contratoPerfilId            : [ perfil.contratoPerfilId ? perfil.contratoPerfilId : 0 ],
              perfilObservacion           : [ ( perfil.contratoPerfilObservacion.length === 0 )
                                                ? 0 : perfil.contratoPerfilObservacion[0].contratoPerfilObservacionId ],
              perfilCodigo                : [ perfil.perfilCodigo ? perfil.perfilCodigo : null ],
              cantidadHvRequeridas        : [ perfil.cantidadHvRequeridas ? perfil.cantidadHvRequeridas : '' ],
              cantidadHvRecibidas         : [ perfil.cantidadHvRecibidas ? perfil.cantidadHvRecibidas : '' ],
              cantidadHvAprobadas         : [ perfil.cantidadHvAprobadas ? perfil.cantidadHvAprobadas : '' ],
              fechaAprobacion             : [ perfil.fechaAprobacion ? new Date( perfil.fechaAprobacion ) : null ],
              observacion                 : [ observaciones ],
              observacionSupervisor       : [ observacionSupervisor ],
              fechaObservacion            : [ fechaObservacion ],
              contratoPerfilNumeroRadicado: this.fb.array( numeroRadicados ),
              rutaSoporte                 : [ perfil.rutaSoporte ? perfil.rutaSoporte : '' ]
            }
          )
        );
      }
      if ( this.perfilesCompletos === this.perfilProyecto.length ) {
        this.perfilesCompletados.emit( 'completo' );
      }
      if ( this.perfilesEnProceso < this.perfilProyecto.length && this.perfilesCompletos !== this.perfilProyecto.length ) {
        this.perfilesCompletados.emit( 'en-proceso' );
      }
    }
  }

  validateNumber( value: string, index: number, campoPerfil: string ) {
    if ( isNaN( Number( value ) ) === true ) {
      this.perfiles.at( index ).get( campoPerfil ).setValue( '' );
    }
  }

  disabledDate( cantidadHvAprobadas: string, cantidadHvRequeridas: string, index: number ) {
    if ( cantidadHvAprobadas >= cantidadHvRequeridas ) {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).enable();
    } else {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
      this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
    }
    if ( cantidadHvRequeridas.length === 0 ) {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
      this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
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

  numeroRadicado( i: number ) {
    return this.perfiles.controls[i].get( 'contratoPerfilNumeroRadicado' ) as FormArray;
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  textoLimpioMessage(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  eliminarPerfil( numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value === true ) {
          this.perfiles.removeAt( numeroPerfil );
          this.formContratista.patchValue({
            numeroPerfiles: `${ this.perfiles.length }`
          });
          this.openDialog( '', 'La información se ha eliminado correctamente.' );
        }
      } );
  }

  deletePerfil( contratoPerfilId: number, numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value === true ) {
          this.faseUnoPreconstruccionSvc.deleteContratoPerfil( contratoPerfilId )
            .subscribe(
              () => {
                this.openDialog( '', 'La información se ha eliminado correctamente.' );
                this.perfiles.removeAt( numeroPerfil );
              },
              err => this.openDialog( '', err.message )
            );
        }
      } );
  }

  agregarNumeroRadicado( numeroRadicado: number, contratoPerfilId: number ) {
    this.numeroRadicado( numeroRadicado ).push(
      this.fb.group({ contratoPerfilNumeroRadicadoId: 0, contratoPerfilId, numeroRadicado: '' })
    );
  }

  eliminarNumeroRadicado( numeroPerfil: number, numeroRadicado ) {
    this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
      .subscribe(
        value => {
          if ( value === true ) {
            this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
            this.openDialog( '', 'La información se ha eliminado correctamente.' );
            return;
          }
        }
      );
  }

  deleteRadicado( contratoPerfilNumeroRadicadoId: number, numeroPerfil: number, numeroRadicado ) {
    if ( contratoPerfilNumeroRadicadoId === 0 ) {
      this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
        .subscribe(
          value => {
            if ( value === true ) {
              this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
              this.openDialog( '', 'La información se ha eliminado correctamente.' );
              return;
            }
          }
        );
      return;
    }
    this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
      .subscribe(
        value => {
          if ( value === true ) {
            this.faseUnoPreconstruccionSvc.deleteContratoPerfilNumeroRadicado( contratoPerfilNumeroRadicadoId )
              .subscribe( () => {
                this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
                this.openDialog( '', 'La información se ha eliminado correctamente.' );
                return;
              } );
          }
        }
      );
  }

  guardar() {
    const perfiles: ContratoPerfil[] = this.formContratista.get( 'perfiles' ).value;

    if ( this.perfilProyecto.length === 0 ) {
      perfiles.forEach( value => {
        value.cantidadHvAprobadas          = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas          = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas         = Number( value.cantidadHvRequeridas );
                                                // tslint:disable-next-line: no-string-literal
        value.contratoPerfilNumeroRadicado = (  value.contratoPerfilNumeroRadicado[0][ 'numeroRadicado' ].length === 0 )
                                                ? null : value.contratoPerfilNumeroRadicado;
        value.contratoPerfilObservacion    = value.observacion ? [{ observacion: value.observacion }] : null;
        value.fechaAprobacion              = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId                   = this.contratoId;
        value.proyectoId                   = this.proyectoId;
      } );
    } else {
      perfiles.forEach( value => {
        value.cantidadHvAprobadas          = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas          = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas         = Number( value.cantidadHvRequeridas );
                                                // tslint:disable-next-line: no-string-literal
        value.contratoPerfilNumeroRadicado = (  value.contratoPerfilNumeroRadicado[0][ 'numeroRadicado' ].length === 0 )
                                                ? null : value.contratoPerfilNumeroRadicado;
        value.contratoPerfilObservacion    = value.observacion ?  [
                                                {
                                                  // tslint:disable-next-line: no-string-literal
                                                  ContratoPerfilObservacionId: value[ 'perfilObservacion' ],
                                                  contratoPerfilId: value.contratoPerfilId,
                                                  observacion: value.observacion
                                                }
                                              ] : null;
        value.fechaAprobacion              = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId                   = this.contratoId;
        value.proyectoId                   = this.proyectoId;
      } );
    }

    console.log( perfiles );
    this.enviarPerfilesContrato.emit( perfiles );
  }

}
