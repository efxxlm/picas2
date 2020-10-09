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

  formContratista        : FormGroup;
  @Input() perfilProyecto: any[] = [];
  @Input() contratoId    : number;
  @Input() proyectoId    : number;
  @Output() enviarPerfilesContrato = new EventEmitter();
  @ViewChild( 'cantidadPerfiles', { static: true } ) cantidadPerfiles: ElementRef;
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
  perfilesCv: Dominio[] = []

  constructor ( private fb                       : FormBuilder,
                private commonSvc                : CommonService,
                private dialog                   : MatDialog,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) 
  {
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
  };

  get perfiles () {
    return this.formContratista.get( 'perfiles' ) as FormArray;
  };

  perfilesProyecto () {
    if ( this.perfilProyecto.length === 0 ) {
      this.formContratista.get( 'numeroPerfiles' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                contratoPerfilId: [ 0 ],
                perfilCodigo: [ null ],
                cantidadHvRequeridas: [ '' ],
                cantidadHvRecibidas: [ '' ],
                cantidadHvAprobadas: [ '' ],
                fechaAprobacion: [ null ],
                observacion: [ null ],
                contratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                rutaSoporte: [ '' ]
              }
            )
          );
        };
      } );
    } else {
      this.formContratista.get( 'numeroPerfiles' ).setValue( String( this.perfilProyecto.length ) );
      this.formContratista.get( 'numeroPerfiles' ).valueChanges
        .subscribe( () => {
          this.cantidadPerfiles.nativeElement.value = String( this.perfilProyecto.length );
        } );
      for ( let perfil of this.perfilProyecto ) {
        let numeroRadicados = [];
        let observaciones = null;
        if ( perfil.contratoPerfilNumeroRadicado.length === 0 ) {
          numeroRadicados.push( 
            this.fb.group(
              {
                contratoPerfilNumeroRadicadoId: 0,
                contratoPerfilId: perfil.contratoPerfilId,
                numeroRadicado: ''
              }
            )
          )
        } else {
          for ( let radicado of perfil.contratoPerfilNumeroRadicado ) {
            numeroRadicados.push( 
              this.fb.group(
                { contratoPerfilNumeroRadicadoId: radicado.contratoPerfilNumeroRadicadoId || 0,
                  contratoPerfilId: perfil.contratoPerfilId,
                  numeroRadicado: radicado.numeroRadicado
                }
              )
            );
          };
        };

        if ( perfil.contratoPerfilObservacion.length > 0 ) {
          for ( let obs of perfil.contratoPerfilObservacion ) {
            if ( obs.tipoObservacionCodigo === '1' ) {
              observaciones = obs.observacion;
            } else if ( obs.tipoObservacionCodigo === '2' ) {
              this.formContratista.get( 'fechaObservacion' ).setValue( obs.fechaCreacion )
              this.formContratista.get( 'observacionSupervisor' ).setValue( obs.observacion );
            }
          }
        }

        this.perfiles.push(
          this.fb.group(
            {
              contratoPerfilId            : [ perfil.contratoPerfilId ],
              perfilObservacion           : [ ( perfil.contratoPerfilObservacion.length === 0 ) ? 0 : perfil.contratoPerfilObservacion[0].contratoPerfilObservacionId ],
              perfilCodigo                : [ perfil.perfilCodigo ],
              cantidadHvRequeridas        : [ String( perfil.cantidadHvRequeridas ) ],
              cantidadHvRecibidas         : [ String( perfil.cantidadHvRecibidas ) ],
              cantidadHvAprobadas         : [ String( perfil.cantidadHvAprobadas ) ],
              fechaAprobacion             : [ new Date( perfil.fechaAprobacion ) ],
              observacion                 : [ observaciones ],
              contratoPerfilNumeroRadicado: this.fb.array( numeroRadicados ),
              rutaSoporte                 : [ perfil.rutaSoporte ]
            }
          )
        )
      };
    };
  };

  disabledDate ( cantidadHvAprobadas: string, cantidadHvRequeridas: string, index: number ) {
    if ( cantidadHvAprobadas >= cantidadHvRequeridas ) {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).enable();
    } else {
      this.perfiles.controls[index].get( 'fechaAprobacion' ).disable();
    }
  };

  openDialog (modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  };

  openDialogTrueFalse (modalTitle: string, modalText: string) {
    
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });

    return dialogRef.afterClosed();
  };

  numeroRadicado ( i: number ) {
    return this.perfiles.controls[i].get( 'contratoPerfilNumeroRadicado' ) as FormArray;
  }

  textoLimpio (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioMessage (texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  maxLength (e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    };
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroPerfiles: [ '' ],
      fechaObservacion: [ null ],
      observacionSupervisor: [ null ],
      perfiles: this.fb.array([])
    });
  };

  eliminarPerfil ( numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value ) {
          this.perfiles.removeAt( numeroPerfil );
          this.formContratista.patchValue({
            numeroPerfiles: `${ this.perfiles.length }`
          });
          this.openDialog( '', 'La información se ha eliminado correctamente.' );
        };
      } );
  };

  deletePerfil( contratoPerfilId: number, numeroPerfil: number ) {
    this.openDialogTrueFalse( '', '¿Está seguro de eliminar esta información?' )
      .subscribe( value => {
        if ( value ) {
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

  agregarNumeroRadicado ( numeroRadicado: number, contratoPerfilId: number ) {
    this.numeroRadicado( numeroRadicado ).push( this.fb.group({ contratoPerfilNumeroRadicadoId: 0, contratoPerfilId: contratoPerfilId, numeroRadicado: '' }) )
  }

  eliminarNumeroRadicado ( numeroPerfil: number, numeroRadicado ) {
    this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
  };

  deleteRadicado ( contratoPerfilNumeroRadicadoId: number, numeroPerfil: number, numeroRadicado ) {
    this.faseUnoPreconstruccionSvc.deleteContratoPerfilNumeroRadicado( contratoPerfilNumeroRadicadoId )
      .subscribe( () => {
        this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
        this.openDialog( '', 'La información se ha eliminado correctamente.' );
      } );
  }

  guardar () {
    let perfiles: ContratoPerfil[] = this.formContratista.get( 'perfiles' ).value;

    if ( this.perfilProyecto.length === 0 ) {
      perfiles.forEach( value => {
        value.cantidadHvAprobadas          = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas          = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas         = Number( value.cantidadHvRequeridas );
        value.contratoPerfilNumeroRadicado = value.contratoPerfilNumeroRadicado;
        value.contratoPerfilObservacion    = [ { observacion: value.observacion } ];
        value.fechaAprobacion              = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId                   = this.contratoId;
        value.proyectoId                   = this.proyectoId;
      } )
    } else {
      perfiles.forEach( value => {
        value.cantidadHvAprobadas          = Number( value.cantidadHvAprobadas );
        value.cantidadHvRecibidas          = Number( value.cantidadHvRecibidas );
        value.cantidadHvRequeridas         = Number( value.cantidadHvRequeridas );
        value.contratoPerfilNumeroRadicado = value.contratoPerfilNumeroRadicado;
        value.contratoPerfilObservacion    =  [ 
                                                { 
                                                  ContratoPerfilObservacionId: value[ 'perfilObservacion' ],
                                                  contratoPerfilId: value.contratoPerfilId,
                                                  observacion: value.observacion 
                                                } 
                                              ];
        value.fechaAprobacion              = value.fechaAprobacion ? new Date( value.fechaAprobacion ).toISOString() : null;
        value.contratoId                   = this.contratoId;
        value.proyectoId                   = this.proyectoId;
      } )
    }

    console.log( perfiles );
    this.enviarPerfilesContrato.emit( perfiles );
  };
  
};