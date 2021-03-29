import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatSelectChange } from '@angular/material/select';
import { AnyAaaaRecord } from 'dns';
import { CommonService } from 'src/app/core/_services/common/common.service';

@Component({
  selector: 'app-mapa-entidad-inst-educativa',
  templateUrl: './mapa-entidad-inst-educativa.component.html',
  styleUrls: ['./mapa-entidad-inst-educativa.component.scss']
})
export class MapaEntidadInstEducativaComponent implements OnInit {
  addressForm: FormGroup = this.fb.group({
    vigencia: [null],
    departamento: [null],
    municipio: [null],
    institucionEducativa: [null]
  });
  vigenciaArray = [
  ];
  departamentoArray = [
  ];
  municipioArray = [
  ];
  institucionEduArray=[

  ];
  gFiltro = false;
  constructor(private fb: FormBuilder, public commonService:CommonService) { }

  ngOnInit(): void {
    this.commonService.listaDepartamentos().subscribe(response=>{
      this.departamentoArray=response;
    });
  }
  getMunicipios(event: MatSelectChange) {
    for(let i = 0; i<event.value.length; i++){
      let arrayFinal = [];
      this.commonService.listaMunicipiosByIdDepartamento(event.value[i].localizacionId).subscribe(respuesta => {
        if(i > 0){
          arrayFinal=respuesta;
          this.municipioArray = this.municipioArray.concat(arrayFinal);
        }
        else{
          this.municipioArray=respuesta;
        }
      });
    }
  }
  onSubmit(){

  }
}
