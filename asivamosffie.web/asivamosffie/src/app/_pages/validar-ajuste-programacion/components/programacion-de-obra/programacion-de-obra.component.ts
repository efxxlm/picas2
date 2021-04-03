import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { FaseUnoConstruccionService } from 'src/app/core/_services/faseUnoConstruccion/fase-uno-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-programacion-de-obra',
  templateUrl: './programacion-de-obra.component.html',
  styleUrls: ['./programacion-de-obra.component.scss']
})
export class ProgramacionDeObraComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required]
  });

  estaEditando = false;

  editorStyle = {
    height: '75px'
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
    public dialog: MatDialog,
    private faseUnoConstruccionService: FaseUnoConstruccionService,
    ) { }

  ngOnInit(): void {
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
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

  onSubmit() {
    // console.log(this.addressForm.value);
    this.estaEditando = true;
    this.addressForm.markAllAsTouched();
    console.log(this.addressForm.value)

    let ajuste = {
      ajusteProgramacionId: 1,
      tieneObservacionesProgramacionObra: this.addressForm.value.tieneObservaciones,

      AjustePragramacionObservacion: [
        {
          ajusteProgramacionId: 1,
          observaciones: this.addressForm.value.observaciones,
          
        }
      ]
    }

    this.faseUnoConstruccionService.CreateEditObservacionAjusteProgramacion( ajuste, true )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message);
        if (respuesta.code === "200")
          this.ngOnInit()
      });

    
  }
  

}
