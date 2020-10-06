import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormArray, FormControl, FormGroup } from '@angular/forms';
import { ContratacionProyecto2 } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { CommonService } from '../../../../core/_services/common/common.service';

@Component({
  selector: 'app-form-perfil',
  templateUrl: './form-perfil.component.html',
  styleUrls: ['./form-perfil.component.scss']
})
export class FormPerfilComponent implements OnInit {

  formContratista: FormGroup;
  @Input() contratacionProyecto: ContratacionProyecto2[] = [];
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
    this.formContratista.get( 'numeroPerfiles' ).valueChanges
      .subscribe( value => {
        this.perfiles.clear();
        for ( let i = 0; i < Number(value); i++ ) {
          this.perfiles.push( 
            this.fb.group(
              {
                tipoPerfil: [ null ],
                cvRequeridas: [ '' ],
                cvRecibidas: [ '' ],
                cvAprobadas: [ '' ],
                fechaAprobacionCv: [ null ],
                observaciones: [ null ],
                numeroRadicadoFfieAprobacionCv: this.fb.array([ [ '' ] ]),
                urlSoporte: [ '' ]
              }
            ) 
          )
        }
      } );
    console.log( this.contratacionProyecto );
  };

  get perfiles () {
    return this.formContratista.get( 'perfiles' ) as FormArray;
  };

  numeroRadicado ( i: number ) {
    return this.perfiles.controls[i].get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray;
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
    console.log( this.perfiles.controls[0].get( 'numeroRadicadoFfieAprobacionCv' ) as FormArray );
    this.numeroRadicado( numeroRadicado ).push( this.fb.control( '' ) )
  }

  eliminarNumeroRadicado ( numeroPerfil: number, numeroRadicado ) {
    this.numeroRadicado( numeroPerfil ).removeAt( numeroRadicado );
  };

  guardar () {
    console.log( this.formContratista );
  }
  
}
