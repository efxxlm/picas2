import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';

@Component({
  selector: 'app-verificar-solicitud-novedad',
  templateUrl: './verificar-solicitud-novedad.component.html',
  styleUrls: ['./verificar-solicitud-novedad.component.scss']
})
export class VerificarSolicitudNovedadComponent implements OnInit {

  estaEditando = false;
  novedad: NovedadContractual;
  detalleId: number;

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required]
  });

  listaObservaciones = [
    {
      fecha: '25/10/2020',
      responsable: 'Supervisor',
      historial: 'Los documentos de soporte no cumplen con los requerimientos solicitados para registrar una novedad.'
    },
    {
      fecha: '20/10/2020',
      responsable: 'Supervisor',
      historial: 'Es necesario revisar a detalle los soportes entregados, por lo tanto, hay que evaluar si en efecto no hay observaciones a la solicitud de novedad.'
    }
  ]

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

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  constructor(
    private fb: FormBuilder,
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private contractualNoveltyService: ContractualNoveltyService,
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.detalleId = params.id;
      //console.log(this.detalleId);

      this.contractualNoveltyService.getNovedadContractualById( this.detalleId )
        .subscribe( respuesta => {
          this.novedad = respuesta;

            this.addressForm.get('observaciones').setValue(this.novedad.observacionApoyo ? this.novedad.observacionApoyo.observaciones : null);
            this.addressForm.get('tieneObservaciones').setValue(this.novedad.tieneObservacionesApoyo);
        });

    });
  }


  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();

    let novedad: NovedadContractual = {
      novedadContractualId: this.detalleId,
      tieneObservacionesApoyo: this.addressForm.value.tieneObservaciones,

      novedadContractualObservaciones: [
        {
          novedadContractualObservacionesId: this.novedad.observacionApoyo ? this.novedad.observacionApoyo.novedadContractualObservacionesId : 0,
          novedadContractualId: this.detalleId,
          esSupervision: false,
          esTramiteNovedades: null,
          observaciones: this.addressForm.value.observaciones
        }
      ]
    }

    this.contractualNoveltyService.createEditObservacion(novedad, false)
      .subscribe(respuesta => {
        this.openDialog('', respuesta.message);
      });
  }

}


