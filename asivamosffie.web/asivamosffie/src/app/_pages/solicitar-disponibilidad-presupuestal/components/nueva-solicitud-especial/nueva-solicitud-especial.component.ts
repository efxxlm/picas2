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
  
  addressForm = this.fb.group({});

  


  constructor(
              private fb: FormBuilder,
              private commonService: CommonService
             ) 
  { }

  ngOnInit(): void {

    this.addressForm = this.createForm();

    this.commonService.listaTipoDisponibilidadPresupuestal().subscribe( respuesta => {
      this.tipoSolicitudArray = respuesta;
    })

  }

  createForm(){
    
    return this.fb.group({
      tipo: [null, Validators.required],
      objetivo: [null, Validators.required],
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
      url: [null, Validators.required],
    });
  }

  onSubmit() {
    console.log(this.addressForm.value);
  }

}
