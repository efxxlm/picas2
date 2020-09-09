import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Dominio, CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-nueva-solicitud-especial',
  templateUrl: './nueva-solicitud-especial.component.html',
  styleUrls: ['./nueva-solicitud-especial.component.scss']
})

export class NuevaSolicitudEspecialComponent implements OnInit {

  tipoSolicitudArray: Dominio[] = [];

  addressForm = this.fb.group({
    tipo: [null, Validators.required],
    objeto: [null, Validators.required],
    numeroRadicado: [null, Validators.compose([
      Validators.minLength(10), Validators.maxLength(15)])],
    cartaAutorizacionET: ['', Validators.required],
    numeroContrato: [null, Validators.compose([
      Validators.minLength(3), Validators.maxLength(10)])],
    departemento: [null, Validators.required],
    municipio: [null, Validators.required],
    llaveMEN: [null, Validators.required],
    tipoAportante: [null, Validators.required],
    nombreAportante: [null, Validators.required],
    valor: [null, Validators.compose([
      Validators.minLength(4), Validators.maxLength(20)])],
    url: [null, Validators.required]
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

  constructor(
    private fb: FormBuilder,
    private commonService: CommonService
  ) { }

  ngOnInit(): void {

    this.commonService.listaTipoDisponibilidadPresupuestal().subscribe(respuesta => {
      this.tipoSolicitudArray = respuesta;
    })

  }

  // createForm() {

  //   return this.fb.group({
  //     tipo: [null, Validators.required],
  //     objetivo: [null, Validators.required],
  //     numeroRadicado: [null, Validators.compose([
  //       Validators.minLength(10), Validators.maxLength(15)])],
  //     cartaAutorizacionET: ['', Validators.required],
  //     numeroContrato: [null, Validators.compose([
  //       Validators.minLength(3), Validators.maxLength(10)])],
  //     departemento: [null, Validators.required],
  //     municipio: [null, Validators.required],
  //     llaveMEN: [null, Validators.required],
  //     tipoAportante: [null, Validators.required],
  //     nombreAportante: [null, Validators.required],
  //     valor: [null, Validators.compose([
  //       Validators.minLength(4), Validators.maxLength(20)])],
  //     url: [null, Validators.required],
  //   });
  // }

  // evalua tecla a tecla
  validateNumberKeypress(event: KeyboardEvent) {
    const alphanumeric = /[0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    return alphanumeric.test(inputChar) ? true : false;
  }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    const textolimpio = texto.replace(/<[^>]*>/g, '');
    return textolimpio.length;
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }

}
