import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Contrato } from 'src/app/_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoPreconstruccionService } from '../../../../core/_services/faseUnoPreconstruccion/fase-uno-preconstruccion.service';
import { ObservacionPerfil } from '../../../../_interfaces/faseUnoVerificarPreconstruccion.interface';
import { ContratoPerfil } from '../../../../_interfaces/faseUnoPreconstruccion.interface';
import { FaseUnoVerificarPreconstruccionService } from '../../../../core/_services/faseUnoVerificarPreconstruccion/fase-uno-verificar-preconstruccion.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Dominio } from 'src/app/core/_services/common/common.service';
import { CommonService } from '../../../../core/_services/common/common.service';

@Component({
  selector: 'app-expansion-verificar-requisitos',
  templateUrl: './expansion-verificar-requisitos.component.html',
  styleUrls: ['./expansion-verificar-requisitos.component.scss']
})
export class ExpansionVerificarRequisitosComponent implements OnInit {

  contrato: Contrato;
  fechaPoliza: string;
  addressForm = this.fb.group({
    tieneObservacion: [null, Validators.required],
    observacion: [null, Validators.required]
  });
  proyectosForm: any[] = [];
  perfilesCv: Dominio[] = [];
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

  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private dialog: MatDialog,
    private routes: Router,
    private commonSvc: CommonService,
    private faseUnoVerificarPreconstruccionSvc: FaseUnoVerificarPreconstruccionService,
    private faseUnoPreconstruccionSvc: FaseUnoPreconstruccionService )
  {
    this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
    if (this.routes.getCurrentNavigation().extras.replaceUrl) {
      this.routes.navigateByUrl('/verificarPreconstruccion');
      return;
    }
    if ( this.routes.getCurrentNavigation().extras.state ) {
      this.fechaPoliza = this.routes.getCurrentNavigation().extras.state.fechaPoliza;
    }
  }

  ngOnInit(): void {
  }

  getContratacionByContratoId( pContratoId: string ) {
    this.commonSvc.listaPerfil()
      .subscribe( perfiles => {
        this.perfilesCv = perfiles;

        this.faseUnoPreconstruccionSvc.getContratacionByContratoId( pContratoId )
          .subscribe( contrato => {
            this.contrato = contrato;
            console.log( this.contrato );
            const observacionTipo2 = [];
            for ( const contratacionProyecto of contrato.contratacion.contratacionProyecto ) {

              let sinDiligenciar = 0;
              let enProceso = 0;
              let completo = 0;

              for ( const perfil of contratacionProyecto.proyecto.contratoPerfil ) {
                // tslint:disable-next-line: no-string-literal
                perfil[ 'tieneObservaciones' ] = null;
                // tslint:disable-next-line: no-string-literal
                perfil[ 'verificarObservacion' ] = '';

                const tipoPerfil = this.perfilesCv.filter( value => value.codigo === perfil.perfilCodigo );
                // tslint:disable-next-line: no-string-literal
                perfil[ 'nombre' ] = tipoPerfil[0].nombre;
                // tslint:disable-next-line: no-string-literal
                if ( perfil[ 'tieneObservacionApoyo' ] === undefined ) {
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'estadoSemaforo' ] = 'sin-diligenciar';
                  sinDiligenciar++;
                }
                // tslint:disable-next-line: no-string-literal
                if ( perfil[ 'tieneObservacionApoyo' ] === false ) {
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'estadoSemaforo' ] = 'completo';
                  // tslint:disable-next-line: no-string-literal
                  perfil[ 'tieneObservaciones' ] = false;
                  completo++;
                }
                for ( const observacionApoyo of perfil.contratoPerfilObservacion ) {
                  if ( observacionApoyo.tipoObservacionCodigo === '2' ) {
                    observacionTipo2.push( observacionApoyo );
                  }
                }
                if ( observacionTipo2.length > 0 ) {
                  // tslint:disable-next-line: no-string-literal
                  if (  perfil[ 'tieneObservacionApoyo' ] === true
                        && observacionTipo2[ observacionTipo2.length - 1 ].observacion === undefined ) {
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'estadoSemaforo' ] = 'en-proceso';
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'tieneObservaciones' ] = true;
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'contratoPerfilObservacionId' ] = observacionTipo2[ observacionTipo2.length - 1 ].contratoPerfilObservacionId;
                    enProceso++;
                  }
                  // tslint:disable-next-line: no-string-literal
                  if (  perfil[ 'tieneObservacionApoyo' ] === true
                        && observacionTipo2[ observacionTipo2.length - 1 ].observacion !== undefined ) {
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'estadoSemaforo' ] = 'completo';
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'tieneObservaciones' ] = true;
                    // tslint:disable-next-line: no-string-literal
                    perfil[ 'verificarObservacion' ] = observacionTipo2[ observacionTipo2.length - 1 ].observacion;
                    completo++;
                  }
                }
              }
              console.log( sinDiligenciar, completo, enProceso );
              if ( sinDiligenciar === contratacionProyecto.proyecto.contratoPerfil.length ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'sin-diligenciar';
              }
              if ( completo === contratacionProyecto.proyecto.contratoPerfil.length ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'completo';
              }
              if (  enProceso > 0
                    || ( completo > 0 && completo < contratacionProyecto.proyecto.contratoPerfil.length )
                    || ( sinDiligenciar > 0 && sinDiligenciar < contratacionProyecto.proyecto.contratoPerfil.length ) ) {
                // tslint:disable-next-line: no-string-literal
                contratacionProyecto[ 'estadoSemaforo' ] = 'en-proceso';
              }
            }
            console.log( this.contrato );
          } );
      } );
  }

  innerObservacion( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  textoLimpio(texto: string) {
    let saltosDeLinea = 0;
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<p');
    saltosDeLinea += this.contarSaltosDeLinea(texto, '<li');

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

  textoLimpioObservacion(texto: string) {
    if ( texto ){
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio;
    }
  }

  onSubmit( perfil: ContratoPerfil ) {
    const observacionPerfil: ObservacionPerfil = {
      contratoPerfilId: perfil.contratoPerfilId,
      // tslint:disable-next-line: no-string-literal
      observacion: perfil[ 'verificarObservacion' ] === null || perfil[ 'verificarObservacion' ].length === 0 ? null : perfil[ 'verificarObservacion' ],
      // tslint:disable-next-line: no-string-literal
      tieneObservacionApoyo: perfil[ 'tieneObservaciones' ]
    };
    // tslint:disable-next-line: no-string-literal
    if ( perfil[ 'contratoPerfilObservacionId' ] !== null ) {
      // tslint:disable-next-line: no-string-literal
      observacionPerfil[ 'contratoPerfilObservacionId' ] = perfil[ 'contratoPerfilObservacionId' ];
    }
    console.log( observacionPerfil );
    this.faseUnoVerificarPreconstruccionSvc.crearContratoPerfilObservacion( observacionPerfil )
      .subscribe(
        response => {
          this.openDialog( '', response.message );
          this.contrato = null;
          this.getContratacionByContratoId( this.activatedRoute.snapshot.params.id );
        },
        err => this.openDialog( '', err.message )
      );
  }

}
