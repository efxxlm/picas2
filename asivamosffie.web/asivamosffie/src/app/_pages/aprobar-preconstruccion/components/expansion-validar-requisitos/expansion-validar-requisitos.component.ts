import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FaseUnoPreconstruccionService } from 'src/app/core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { FaseUnoVerificarPreconstruccionService } from 'src/app/core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { Contrato, ContratoPerfil } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { ObservacionPerfil } from 'src/app/_interfaces/faseUnoVerificarPreconstruccion.interface';

@Component({
  selector: 'app-expansion-validar-requisitos',
  templateUrl: './expansion-validar-requisitos.component.html',
  styleUrls: ['./expansion-validar-requisitos.component.scss']
})
export class ExpansionValidarRequisitosComponent implements OnInit {

  contrato: Contrato;
  addressForm = this.fb.group({
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required]
  });

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

  constructor ( private fb: FormBuilder,
                private activatedRoute: ActivatedRoute,
                private faseUnoVerificarPreconstruccionSvc: FaseUnoVerificarPreconstruccionService,
                private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService )
  { 
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId ( pContratoId: string ) {
    this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
      .subscribe( contrato => {
        this.contrato = contrato;
        for ( let contratacionProyecto of contrato.contratacion.contratacionProyecto ) {

          for ( let perfil of contratacionProyecto.proyecto.contratoPerfil ) {
            perfil[ 'tieneObservaciones' ] = null;
            perfil[ 'verificarObservacion' ] = '';
          };

        };
        console.log( this.contrato );
      } );
  };

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    };
  };

  textoLimpioObservacion(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    };
  };

  onSubmit( perfil: ContratoPerfil ) {
    const observacionPerfil: ObservacionPerfil = {
      contratoPerfilId: perfil.contratoPerfilId,
      observacion: perfil[ 'verificarObservacion' ].length === 0 ? null : perfil[ 'verificarObservacion' ],
      tieneObservacionSupervisor: perfil[ 'tieneObservaciones' ]
    };
    console.log( observacionPerfil );
    this.faseUnoVerificarPreconstruccionSvc.aprobarCrearContratoPerfilObservacion( observacionPerfil )
      .subscribe( console.log );
  }

}
