import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormGroup } from '@angular/forms';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';

@Component({
  selector: 'app-form-perfil',
  templateUrl: './form-perfil.component.html',
  styleUrls: ['./form-perfil.component.scss']
})
export class FormPerfilComponent implements OnInit {

  formContratista: FormGroup;
  tieneEstadoFase1EyD: boolean = false;
  tieneEstadoFase1Diagnostico: boolean = false;
  @Input() perfilProyecto: any[] = [];
  @Input() contratoId: number;
  @Input() proyectoId: number;
  @Output() enviarPerfilesContrato = new EventEmitter();
  @ViewChild( 'cantidadPerfiles', { static: true } ) cantidadPerfiles: ElementRef;
  editorStyle = {
    height: '45px'
  };
  estadoProyectoArray = [
    {
      name: 'Estudios y Diseños'
    },
    {
      name: 'Diagnóstico'
    }
  ];
  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  perfilesCv: any[] = [
    { value: 'Ingeniero de obra' },
    { value: 'Ingeniero electrico' }
  ]
  get perfiles () {
    return this.formContratista.get( 'perfiles' ) as FormArray;
  };

  constructor ( private fb: FormBuilder,
                private commonSvc: CommonService,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.perfilesProyecto();
  };

  crearFormulario () {
    this.formContratista = this.fb.group({
      numeroPerfiles: [ '' ],
      perfiles: this.fb.array([])
    });
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
                contratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ contratoPerfilNumeroRadicadoId: 0, contratoPerfilId: 0, numeroRadicado: '' }) ]),
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
        //console.log( perfil.contratoPerfilNumeroRadicado );
        let numeroRadicados = [];
        if ( perfil.contratoPerfilNumeroRadicado.length === 0 ) {
          numeroRadicados.push( this.fb.group({ contratoPerfilNumeroRadicadoId: 0, contratoPerfilId: perfil.contratoPerfilId, numeroRadicado: '' }) )
        } else {
          for ( let radicado of perfil.contratoPerfilNumeroRadicado ) {
            numeroRadicados.push( this.fb.group({ contratoPerfilNumeroRadicadoId: radicado.contratoPerfilNumeroRadicadoId || 0, contratoPerfilId: perfil.contratoPerfilId, numeroRadicado: radicado.numeroRadicado }) );
          };
        }
        //console.log( numeroRadicados );
        this.perfiles.push(
          this.fb.group(
            {
              contratoPerfilId: [ perfil.contratoPerfilId ],
              perfilObservacion: [ ( perfil.contratoPerfilObservacion.length === 0 ) ? 0 : perfil.contratoPerfilObservacion[0].contratoPerfilObservacionId ],
              perfilCodigo: [ perfil.perfilCodigo ],
              cantidadHvRequeridas: [ String( perfil.cantidadHvRequeridas ) ],
              cantidadHvRecibidas: [ String( perfil.cantidadHvRecibidas ) ],
              cantidadHvAprobadas: [ String( perfil.cantidadHvAprobadas ) ],
              fechaAprobacion: [ new Date( perfil.fechaAprobacion ) ],
              observacion: [ perfil.contratoPerfilObservacion[0]?.observacion ],
              contratoPerfilNumeroRadicado: this.fb.array( numeroRadicados ),
              rutaSoporte: [ perfil.rutaSoporte ]
            }
          )
        )
      };
    };
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

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroPerfiles: `${ this.perfiles.length }`
    });
  };

  agregarNumeroRadicado ( numeroRadicado: number, contratoPerfilId: number ) {
    this.numeroRadicado( numeroRadicado ).push( this.fb.group({ contratoPerfilNumeroRadicadoId: 0, contratoPerfilId: contratoPerfilId, numeroRadicado: '' }) )
  }

  eliminarNumeroRadicado ( numeroPerfil: number, numeroRadicado ) {
    this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
  };

  deleteRadicado ( contratoPerfilNumeroRadicadoId: number ) {
    console.log( 'probando metodo - radicado' );
    this.faseUnoPreconstruccionSvc.deleteContratoPerfilNumeroRadicado( contratoPerfilNumeroRadicadoId )
      .subscribe( console.log );
  }

  deletePerfil( contratoPerfilId: number ) {
    console.log( 'probando metodo - perfil', contratoPerfilId );
    this.faseUnoPreconstruccionSvc.deleteContratoPerfil( contratoPerfilId )
      .subscribe( console.log );
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
        value.fechaAprobacion              = new Date( value.fechaAprobacion ).toISOString()
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
        value.fechaAprobacion              = new Date( value.fechaAprobacion ).toISOString()
        value.contratoId                   = this.contratoId;
        value.proyectoId                   = this.proyectoId;
      } )
    }

    console.log( perfiles );
    this.enviarPerfilesContrato.emit( { 
      perfiles,
      tieneEstadoFase1EyD: this.tieneEstadoFase1EyD,
      tieneEstadoFase1Diagnostico: this.tieneEstadoFase1Diagnostico
    } );
  };

}
