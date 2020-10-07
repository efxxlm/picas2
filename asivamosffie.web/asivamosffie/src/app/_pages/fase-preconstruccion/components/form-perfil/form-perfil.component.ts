import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup } from '@angular/forms';
import { ContratacionProyecto2, ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { CommonService } from '../../../../core/_services/common/common.service';

@Component({
  selector: 'app-form-perfil',
  templateUrl: './form-perfil.component.html',
  styleUrls: ['./form-perfil.component.scss']
})
export class FormPerfilComponent implements OnInit {

  formContratista: FormGroup;
  @Input() perfilProyecto: any[] = [];
  @Input() contratoId: number;
  @Input() proyectoId: number;
  @Output() enviarPerfilesContrato = new EventEmitter();
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
  perfilesCv: any[] = [
    { value: 'Ingeniero de obra' },
    { value: 'Ingeniero electrico' }
  ]

  constructor ( private fb: FormBuilder,
                private commonSvc: CommonService ) {
    this.crearFormulario();
  }

  ngOnInit(): void {
    this.perfilesProyecto();
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
                perfilCodigo: [ null ],
                cantidadHvRequeridas: [ '' ],
                cantidadHvRecibidas: [ '' ],
                cantidadHvAprobadas: [ '' ],
                fechaAprobacion: [ null ],
                observacion: [ null ],
                ContratoPerfilNumeroRadicado: this.fb.array([ this.fb.group({ numeroRadicado: '' }) ]),
                rutaSoporte: [ '' ]
              }
            )
          );
        };
      } );
    };
  };

  numeroRadicado ( i: number ) {
    return this.perfiles.controls[i].get( 'ContratoPerfilNumeroRadicado' ) as FormArray;
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
      perfiles: this.fb.array([])
    });
  };

  eliminarPerfil ( numeroPerfil: number ) {
    this.perfiles.removeAt( numeroPerfil );
    this.formContratista.patchValue({
      numeroPerfiles: `${ this.perfiles.length }`
    });
  };

  agregarNumeroRadicado ( numeroRadicado: number ) {
    this.numeroRadicado( numeroRadicado ).push( this.fb.group({ numeroRadicado: [ '' ] }) )
  }

  eliminarNumeroRadicado ( numeroPerfil: number, numeroRadicado ) {
    this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
  };

  guardar () {
    let perfiles: ContratoPerfil[] = this.formContratista.get( 'perfiles' ).value;

    perfiles.forEach( value => {
      value.cantidadHvAprobadas          = Number( value.cantidadHvAprobadas );
      value.cantidadHvRecibidas          = Number( value.cantidadHvRecibidas );
      value.cantidadHvRequeridas         = Number( value.cantidadHvRequeridas );
      value.ContratoPerfilNumeroRadicado = value.ContratoPerfilNumeroRadicado;
      value.fechaAprobacion              = new Date( value.fechaAprobacion ).toISOString()
      value.contratoId                   = this.contratoId;
      value.proyectoId                   = this.proyectoId;
    } )

    console.log( perfiles );
    this.enviarPerfilesContrato.emit( perfiles );
  };
  
};