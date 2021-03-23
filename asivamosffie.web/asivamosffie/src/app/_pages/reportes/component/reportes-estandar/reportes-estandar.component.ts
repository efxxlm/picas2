import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-reportes-estandar',
  templateUrl: './reportes-estandar.component.html',
  styleUrls: ['./reportes-estandar.component.scss']
})
export class ReportesEstandarComponent implements OnInit {

  addressForm: FormGroup = this.fb.group({
    etapa: [null],
    procesoAcuerdo: [null],
    reporte: [null],
    departamento: [null],
    municipio: [null]
  });
  etapaArray = [
    {
      name: 'Inicio',
      value: 'Inicio',
      code:'1'
    },
    {
      name: 'Seguimiento',
      value: 'Seguimiento',
      code:'2'
    },
    {
      name: 'Liquidación',
      value: 'Liquidación',
      code:'3'
    },
    {
      name: 'Comités',
      value: 'Comités',
      code:'4'
    }
  ];
  procesoAcuerdoArray = [
    {
      name: 'Facturación',
      value: 'Facturación',
      code:'1'
    },
    {
      name: 'Seguimiento técnico',
      value: 'Seguimiento técnico',
      code:'2'
    },
    {
      name: 'Seguimiento financiero',
      value: 'Seguimiento financiero',
      code:'3'
    },
    {
      name: 'Presupuestales',
      value: 'Presupuestales',
      code:'4'
    },
    {
      name: 'Novedades',
      value: 'Novedades',
      code:'5'
    },
    {
      name: 'Controversias',
      value: 'Controversias',
      code:'6'
    },
    {
      name: 'Procesos de defensa judicial',
      value: 'Procesos de defensa judicial',
      code:'7'
    },
    {
      name: 'Contratación',
      value: 'Contratación',
      code:'8'
    }
  ];
  reporteArray = [
    {
      name: 'Estado de ordenes de giro',
      value: 'Estado de ordenes de giro',
      code:'1'
    },
    {
      name: 'Estado de rendimientos',
      value: 'Estado de rendimientos',
      code:'2'
    },
    {
      name: 'Estado de fuentes de financiación',
      value: 'Estado de fuentes de financiación',
      code:'3'
    },
    {
      name: 'Estado de disponibilidades',
      value: 'Estado de disponibilidades',
      code:'4'
    },
    {
      name: 'Novedades',
      value: 'Novedades',
      code:'5'
    },
    {
      name: 'Controversias',
      value: 'Controversias',
      code:'6'
    },
    {
      name: 'Procesos de defensa judicial',
      value: 'Procesos de defensa judicial',
      code:'7'
    },
  ];
  departamentoArray = [
  ];
  municipioArray = [
  ];
  constructor(private fb: FormBuilder,public commonService:CommonService) { }

  ngOnInit(): void {
    this.commonService.listaDepartamentos().subscribe(response=>{
      this.departamentoArray=response;
    });
  }
  getMunicipio(event: MatSelectChange) {
    this.commonService.listaMunicipiosByIdDepartamento(event.value).subscribe(respuesta => {
      this.municipioArray = respuesta;
    });
  }
  onSubmit(){

  }
}
