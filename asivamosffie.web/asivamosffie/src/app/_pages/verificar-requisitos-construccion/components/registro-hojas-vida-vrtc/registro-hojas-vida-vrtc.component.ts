import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-registro-hojas-vida-vrtc',
  templateUrl: './registro-hojas-vida-vrtc.component.html',
  styleUrls: ['./registro-hojas-vida-vrtc.component.scss']
})
export class RegistroHojasVidaVrtcComponent implements OnInit {

  formContratista: FormGroup;
  minDate: Date;
  estaEditando = false;

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
  perfilesCv: Dominio[] = [];
  get perfiles() {
    return this.formContratista.get( 'perfiles' ) as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private commonSvc: CommonService,
    private faseUnoConstruccionSvc: FaseUnoConstruccionService,
    private dialog: MatDialog )
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
    if ( this.perfilProyecto.length === 0 ) {
      this.formContratista.get( 'numeroPerfiles' ).valueChanges
      .subscribe( value => {
        if ( this.formContratista.get( 'perfiles' ).dirty === true && Number( value ) > 0 ){
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
                  observaciones               : [ null ],
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
                  observaciones               : [ null ],
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
      this.estaEditando = true;
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
                    estadoSemaforo              : [ 'sin-diligenciar' ],
                    contratoPerfilId            : [ 0 ],
                    perfilCodigo                : [ null ],
                    cantidadHvRequeridas        : [ '' ],
                    cantidadHvRecibidas         : [ '' ],
                    cantidadHvAprobadas         : [ '' ],
                    fechaAprobacion             : [ null ],
                    observaciones               : [ null ],
                    observacionSupervisor       : [ null ],
                    fechaObservacion            : [ null ],
                    contratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                    rutaSoporte                 : [ '' ]
                  }
                )
              );
            }
          });
      for ( const perfil of this.perfilProyecto ) {
        const numeroRadicados = [];
        let observaciones = null;
        let fechaObservacion = null;
        let observacionSupervisor = null;
        let semaforo;
        if ( perfil['construccionPerfilNumeroRadicado'].length === 0 ) {
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
          for ( const radicado of perfil['construccionPerfilNumeroRadicado'] ) {
            numeroRadicados.push(
              this.fb.group(
                { construccionPerfilNumeroRadicadoId: radicado['construccionPerfilNumeroRadicadoId'] || 0,
                  contratoPerfilId: perfil.contratoPerfilId,
                  numeroRadicado: radicado.numeroRadicado
                }
              )
            );
          }
        }

        if ( perfil['construccionPerfilObservacion'].length > 0 ) {
          for ( const obs of perfil['construccionPerfilObservacion'] ) {
            if ( obs.tipoObservacionCodigo === '1' ) {
              observaciones = perfil['observaciones'];
            } else if ( obs.tipoObservacionCodigo === '3' ) {
              fechaObservacion = obs.fechaCreacion;
              observacionSupervisor = obs.observacion;
            }
          }
        }

        if ( perfil.registroCompleto ) {
          this.perfilesCompletos++;
          semaforo = 'completo';
        }
        if (  perfil.registroCompleto === false
              && perfil.perfilCodigo !== undefined
              && (  perfil.construccionPerfilId !== undefined
                    || perfil.cantidadHvRequeridas > 0
                    || perfil.cantidadHvRecibidas > 0
                    || perfil.cantidadHvAprobadas > 0  ) ) {
          semaforo = 'en-proceso';
          this.perfilesEnProceso++;
        }
        console.log( perfil, semaforo );
        this.perfiles.push(
          this.fb.group(
            {
              estadoSemaforo              : [ semaforo || 'sin-diligenciar' ],
              contratoPerfilId            : [ perfil.construccionPerfilId ? perfil.construccionPerfilId : 0 ],
              perfilObservacion           : [ ( perfil['construccionPerfilObservacion'].length === 0 ) ? 0 : perfil['construccionPerfilObservacion'][0].contratoPerfilObservacionId ],
              perfilCodigo                : [ perfil.perfilCodigo ? perfil.perfilCodigo : null ],
              cantidadHvRequeridas        : [ perfil.cantidadHvRequeridas ? String( perfil.cantidadHvRequeridas ) : '' ],
              cantidadHvRecibidas         : [ perfil.cantidadHvRecibidas ? String( perfil.cantidadHvRecibidas ) : '' ],
              cantidadHvAprobadas         : [ perfil.cantidadHvAprobadas ? String( perfil.cantidadHvAprobadas ) : '' ],
              fechaAprobacion             : [ perfil.fechaAprobacion ? new Date( perfil.fechaAprobacion ) : null ],
              observaciones               : [ perfil.observaciones ],
              observacionDevolucion       : [ perfil.observacionDevolucion],
              observacionSupervisor       : [ observacionSupervisor ],
              fechaObservacion            : [ fechaObservacion ],
              contratoPerfilNumeroRadicado: this.fb.array( numeroRadicados ),
              rutaSoporte                 : [ perfil.rutaSoporte ? perfil.rutaSoporte : '' ]

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
    }
  }

  disabledDate( cantidadHvAprobadas: string, cantidadHvRequeridas: string, cantidadHvRecibidas: string, index: number ) {
    if ( Number( cantidadHvAprobadas ) >= Number( cantidadHvRequeridas ) ) {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).enable();
    } else {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
      this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
    }
    if ( cantidadHvRequeridas === null ) {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
      this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
    }
    if (Number( cantidadHvAprobadas ) > Number( cantidadHvRecibidas )){
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
      this.perfiles.controls[index].get( 'fechaAprobacion' ).setValue( null );
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
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
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p>');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li>');

    if ( texto ){
      const textolimpio = texto.replace(/<(?:.|\n)*?>/gm, '');
      return textolimpio.length + saltosDeLinea;
    }
  }

  private contarSaltosDeLinea(cadena: string, subcadena: string) {
    let contadorConcurrencias = 0;
    let posicion = 0;
    while ((posicion = cadena.indexOf(subcadena, posicion)) !== -1) {
      ++contadorConcurrencias;
      posicion += subcadena.length;
    }
    return contadorConcurrencias;
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
    this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
      .subscribe( value => {
        if ( value ) {
          this.perfiles.removeAt( numeroPerfil );
          this.formContratista.patchValue({
            numeroPerfiles: `${ this.perfiles.length }`
          });
          this.openDialog( '', '<b>La información se ha eliminado correctamente.</b' );
        }
      } );
  }

  deletePerfil( contratoPerfilId: number, numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '<b>¿Está seguro de eliminar esta información?</b>' )
      .subscribe( value => {
        if ( value === true ) {
          if ( contratoPerfilId !== 0 ) {
            this.faseUnoConstruccionSvc.deleteConstruccionPerfil( contratoPerfilId )
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
      this.fb.group({ contratoPerfilNumeroRadicadoId: 0, contratoPerfilId, numeroRadicado: '' })
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
            this.faseUnoConstruccionSvc.deleteConstruccionPerfilNumeroRadicado( contratoPerfilNumeroRadicadoId )
              .subscribe( () => {
                this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
                this.openDialog( '', '<b>La información se ha eliminado correctamente.</b>' );
                return;
              } );
          }
        }
      );
  }

  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    // tslint:disable-next-line: deprecation
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }


  guardar() {
    const perfiles: ContratoPerfil[] = this.formContratista.get( 'perfiles' ).value;

    if ( this.perfilProyecto.length === 0 ) {
      perfiles.forEach( value => {
        value.cantidadHvAprobadas                 = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas                 = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas                = Number( value.cantidadHvRequeridas );
        value['construccionPerfilNumeroRadicado'] = ( value.contratoPerfilNumeroRadicado[0][ 'numeroRadicado' ].length === 0 )
                                                      ? null : value.contratoPerfilNumeroRadicado;
        value['observaciones']                    = value.observaciones;
        value.fechaAprobacion                     = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId                          = this.contratoId;
        value.proyectoId                          = this.proyectoId;
      } );
    } else {
      perfiles.forEach( value => {

        value['construccionPerfilId']             = value.contratoPerfilId;
        value.cantidadHvAprobadas                 = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas                 = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas                = Number( value.cantidadHvRequeridas );
        value['construccionPerfilNumeroRadicado'] = ( value.contratoPerfilNumeroRadicado[0][ 'numeroRadicado' ].length === 0 )
                                                      ? null : value.contratoPerfilNumeroRadicado;
        value['observaciones']                    = value.observaciones;
        value.fechaAprobacion                     = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId                          = this.contratoId;
        value.proyectoId                          = this.proyectoId;
      } );
    }

    console.log( perfiles );
    this.enviarPerfilesContrato.emit( perfiles );
  }


}
