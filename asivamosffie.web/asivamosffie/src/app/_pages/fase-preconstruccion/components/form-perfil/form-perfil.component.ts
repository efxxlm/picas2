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
  @Output() perfilEliminado = new EventEmitter();
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
  listaTipoObservaciones = {
    obsInterventor: '1',
    obsSupervisor: '3'
  }
  perfilesCv: Dominio[] = [];
  estaEditando = false;

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
    this.perfilesProyecto();
  }

  crearFormulario() {
    this.formContratista = this.fb.group({
      numeroPerfiles: [ '' ],
      perfiles: this.fb.array([])
    });
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
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
                  estadoSemaforo                : [ 'sin-diligenciar', Validators.required ],
                  contratoPerfilId              : [ 0, Validators.required ],
                  perfilCodigo                  : [ null, Validators.required ],
                  cantidadHvRequeridas          : [ null, Validators.required ],
                  cantidadHvRecibidas           : [ null, Validators.required ],
                  cantidadHvAprobadas           : [ null, Validators.required ],
                  fechaAprobacion               : [ null, Validators.required ],
                  observacion                   : [ null, Validators.required ],
                  observacionSupervisor         : [ null, Validators.required ],
                  fechaObservacion              : [ null, Validators.required ],
                  contratoPerfilObservacionArray: [ [] ],
                  contratoPerfilNumeroRadicado  : this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                  rutaSoporte                   : [ '', Validators.required ]
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
                  estadoSemaforo                : [ 'sin-diligenciar', Validators.required ],
                  contratoPerfilId              : [ 0, Validators.required ],
                  perfilCodigo                  : [ null, Validators.required ],
                  cantidadHvRequeridas          : [ null, Validators.required ],
                  cantidadHvRecibidas           : [ null, Validators.required ],
                  cantidadHvAprobadas           : [ null, Validators.required ],
                  fechaAprobacion               : [ null, Validators.required ],
                  observacion                   : [ null, Validators.required ],
                  observacionSupervisor         : [ null, Validators.required ],
                  fechaObservacion              : [ null, Validators.required ],
                  contratoPerfilObservacionArray: [ [] ],
                  contratoPerfilNumeroRadicado  : this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                  rutaSoporte                   : [ '', Validators.required ]
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
            if ( value < this.perfiles.length && value > 0 ) {
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
                    estadoSemaforo                : [ 'sin-diligenciar', Validators.required ],
                    contratoPerfilId              : [ 0, Validators.required ],
                    perfilCodigo                  : [ null, Validators.required ],
                    cantidadHvRequeridas          : [ null, Validators.required ],
                    cantidadHvRecibidas           : [ null, Validators.required ],
                    cantidadHvAprobadas           : [ null, Validators.required ],
                    fechaAprobacion               : [ null, Validators.required ],
                    observacion                   : [ null, Validators.required ],
                    observacionSupervisor         : [ null, Validators.required ],
                    fechaObservacion              : [ null, Validators.required ],
                    contratoPerfilObservacionArray: [ [] ],
                    contratoPerfilNumeroRadicado  : this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                    rutaSoporte                   : [ '', Validators.required ]
                  }
                )
              );
            }
          }
        );
      for ( const perfil of this.perfilProyecto ) {
        const numeroRadicados = [];
        const observacionInterventor = [];
        let observacionSupervisor = [];
        let semaforo;
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
          observacionSupervisor = perfil.contratoPerfilObservacion.filter( obs => obs.tipoObservacionCodigo === this.listaTipoObservaciones.obsSupervisor && perfil.tieneObservacionSupervisor === true )
        }
        if ( perfil.registroCompleto === true ) {
          if ( perfil.tieneObservacionSupervisor === true ) {
            semaforo = 'en-proceso';
            this.perfilesEnProceso++;
          } else {
            this.perfilesCompletos++;
            semaforo = 'completo';
          }
        }
        if (  perfil.registroCompleto === false
              && perfil.perfilCodigo !== undefined
              && (  perfil.contratoPerfilId !== undefined
                    || perfil.cantidadHvRequeridas > 0
                    || perfil.cantidadHvRecibidas > 0
                    || perfil.cantidadHvAprobadas > 0 ) ) {
          semaforo = 'en-proceso';
          this.perfilesEnProceso++;
        }
        console.log( perfil.observacion, perfil )
        this.perfiles.push(
          this.fb.group(
            {
              estadoSemaforo                : [ perfil.tieneObservacionSupervisor === true ? 'en-proceso' : ( semaforo ? semaforo : 'sin-diligenciar' ), Validators.required ],
              contratoPerfilId              : [ perfil.contratoPerfilId ? perfil.contratoPerfilId : 0, Validators.required ],
              perfilCodigo                  : [ perfil.perfilCodigo ? perfil.perfilCodigo : null, Validators.required ],
              cantidadHvRequeridas          : [ perfil.cantidadHvRequeridas ? perfil.cantidadHvRequeridas : null, Validators.required ],
              cantidadHvRecibidas           : [ perfil.cantidadHvRecibidas ? perfil.cantidadHvRecibidas : null, Validators.required ],
              cantidadHvAprobadas           : [ perfil.cantidadHvAprobadas ? perfil.cantidadHvAprobadas : null, Validators.required ],
              fechaAprobacion               : [ perfil.fechaAprobacion ? new Date( perfil.fechaAprobacion ) : null ],
              tieneObservacionSupervisor    : [ perfil.tieneObservacionSupervisor !== undefined ? perfil.tieneObservacionSupervisor : null ],
              observacion                   : [ perfil.observacion !== undefined ? perfil.observacion : null, Validators.required ],
              observacionSupervisor         : [ observacionSupervisor.length > 0 ? observacionSupervisor[ observacionSupervisor.length - 1 ].observacion : null, Validators.required ],
              contratoPerfilObservacionArray: [ perfil.contratoPerfilObservacion.length > 0 ? perfil.contratoPerfilObservacion : [] ],
              fechaObservacion              : [ observacionSupervisor.length > 0 ? observacionSupervisor[ observacionSupervisor.length - 1 ].fechaCreacion : null, Validators.required ],
              contratoPerfilNumeroRadicado  : this.fb.array( numeroRadicados),
              rutaSoporte                   : [ perfil.rutaSoporte ? perfil.rutaSoporte : '', Validators.required ]
            }
          )
        );
      }
      if ( this.perfilesCompletos > 0 && this.perfilesCompletos === this.perfilProyecto.length ) {
        this.perfilesCompletados.emit( 'completo' );
      }
      if (  this.perfilesEnProceso > 0
            || ( this.perfilesEnProceso === 0 && this.perfilesCompletos > 0 && this.perfilesCompletos < this.perfilProyecto.length ) ) {
        this.perfilesCompletados.emit( 'en-proceso' );
      }
      if ( this.perfilesCompletos === 0 && this.perfilesEnProceso === 0 && this.perfilProyecto.length > 0 ) {
        this.perfilesCompletados.emit( 'sin-diligenciar' );
      }
      this.estaEditando = true;
      this.formContratista.markAllAsTouched();
    }
  }

  validateNumber( value: string, index: number, campoPerfil: string ) {
    if ( isNaN( Number( value ) ) === true ) {
      this.perfiles.at( index ).get( campoPerfil ).setValue( '' );
    }
  }

  disabledDate( cantidadHvAprobadas: string, cantidadHvRequeridas: string, cantidadHvRecibidas: string, index: number ) {
    if ( cantidadHvRequeridas === null ) {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
      this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
    }
    else {
      if ( Number( cantidadHvAprobadas ) === Number( cantidadHvRequeridas ) ) {
        this.perfiles.controls[index].get( 'fechaAprobacion' ).enable();
      } else {
        this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
        this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
      }
      if (Number( cantidadHvAprobadas ) > Number( cantidadHvRecibidas )){
        this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
        this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
      }
      if (Number( cantidadHvAprobadas ) > Number( cantidadHvRequeridas )) {
        this.perfiles.controls[index].get( 'fechaAprobacion' ).enable();
      }
    }
  }

  checkDateAprove( value: FormControl ) {
    console.log( value )
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

  textoLimpioMessage(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  eliminarPerfil( numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
      .subscribe( value => {
        if ( value === true ) {
          this.perfiles.removeAt( numeroPerfil );
          this.formContratista.patchValue({
            numeroPerfiles: `${ this.perfiles.length }`
          });
          this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
        }
      } );
  }

  deletePerfil( contratoPerfilId: number, numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
      .subscribe( value => {
        if ( value === true ) {
          if ( contratoPerfilId !== 0 ) {
            this.faseUnoPreconstruccionSvc.deleteContratoPerfil( contratoPerfilId )
            .subscribe(
              () => {
                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                this.perfiles.removeAt( numeroPerfil );
                this.perfilEliminado.emit( true );
                this.formContratista.patchValue({
                  numeroPerfiles: `${ this.perfiles.length }`
                });
              },
              err => this.openDialog( '', err.message )
            );
          } else {
            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
            this.perfiles.removeAt( numeroPerfil );
            this.formContratista.patchValue({
              numeroPerfiles: `${ this.perfiles.length }`
            });
          }
        }
      } );
  }

  agregarNumeroRadicado( numeroRadicado: number, contratoPerfilId: number ) {
    this.numeroRadicado( numeroRadicado ).push(
      this.fb.group({
        contratoPerfilNumeroRadicadoId: 0,
        contratoPerfilId,
        numeroRadicado: ['', Validators.required] })
    );
  }

  eliminarNumeroRadicado( numeroPerfil: number, numeroRadicado ) {
    this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
      .subscribe(
        value => {
          if ( value === true ) {
            this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
            this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
            return;
          }
        }
      );
  }

  deleteRadicado( contratoPerfilNumeroRadicadoId: number, numeroPerfil: number, numeroRadicado ) {
    if ( contratoPerfilNumeroRadicadoId === 0 ) {
      this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
        .subscribe(
          value => {
            if ( value === true ) {
              this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
              this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
              return;
            }
          }
        );
      return;
    }
    this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
      .subscribe(
        value => {
          if ( value === true ) {
            this.faseUnoPreconstruccionSvc.deleteContratoPerfilNumeroRadicado( contratoPerfilNumeroRadicadoId )
              .subscribe( () => {
                this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                return;
              } );
          }
        }
      );
  }

  guardar() {
    this.estaEditando = true;
    this.formContratista.markAllAsTouched();
    const perfiles: ContratoPerfil[] = this.formContratista.get( 'perfiles' ).value;
    const perfilesArray = [];

    if ( this.perfilProyecto.length === 0 ) {
      perfiles.forEach( value => {
        value.cantidadHvAprobadas = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas = Number( value.cantidadHvRequeridas );
        value.contratoPerfilNumeroRadicado = ( value.contratoPerfilNumeroRadicado[0][ 'numeroRadicado' ].length === 0 ) ? null : value.contratoPerfilNumeroRadicado;
        value.fechaAprobacion = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId = this.contratoId;
        value.proyectoId = this.proyectoId;
      } );
    } else {
      this.perfiles.controls.forEach( perfil => {

        perfilesArray.push(
          {
            tieneObservacionSupervisor: perfil.dirty === true && perfil.get( 'observacionSupervisor' ).value !== null ? false : null,
            cantidadHvAprobadas: Number( perfil.get( 'cantidadHvAprobadas' ).value ),
            cantidadHvRecibidas: Number( perfil.get( 'cantidadHvRecibidas' ).value ),
            cantidadHvRequeridas: Number( perfil.get( 'cantidadHvRequeridas' ).value ),
            contratoPerfilNumeroRadicado: perfil.get( 'contratoPerfilNumeroRadicado' ).value[0].length === 0 ? null : perfil.get( 'contratoPerfilNumeroRadicado' ).value,
            observacion: perfil.get( 'observacion' ).value,
            fechaAprobacion: perfil.get( 'fechaAprobacion' ).value !== null ? new Date( perfil.get( 'fechaAprobacion' ).value ).toISOString() : perfil.get( 'fechaAprobacion' ).value,
            contratoPerfilId: perfil.get( 'contratoPerfilId' ).value,
            perfilCodigo: perfil.get( 'perfilCodigo' ).value,
            rutaSoporte: perfil.get( 'rutaSoporte' ).value,
            contratoId: this.contratoId,
            proyectoId: this.proyectoId,
          }
        );
      } );
    }

    this.enviarPerfilesContrato.emit( this.perfilProyecto.length === 0 ? perfiles : perfilesArray );
  }

}
