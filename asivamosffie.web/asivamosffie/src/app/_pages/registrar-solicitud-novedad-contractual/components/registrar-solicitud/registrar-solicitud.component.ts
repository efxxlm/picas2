import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { CommonService } from 'src/app/core/_services/common/common.service';
import { ContratosModificacionesContractualesService } from 'src/app/core/_services/contratos-modificaciones-contractuales/contratos-modificaciones-contractuales.service';

@Component({
  selector: 'app-registrar-solicitud',
  templateUrl: './registrar-solicitud.component.html',
  styleUrls: ['./registrar-solicitud.component.scss']
})
export class RegistrarSolicitudComponent implements OnInit {

  numeroContrato = new FormControl();
  novedadAplicada = new FormControl();
  filteredOptions: Observable<string[]>;
  options: string[] = [];

  novedadesArray = [
    { name: 'Contrato', value: true },
    { name: 'Proyecto', value: false }
  ];

  constructor(public contratoServices: ContratosModificacionesContractualesService) { 
    
  }
  numeroContratoSeleccionado="";
  proyectos=[];
  contrato=null;
  contratos=[];

  ngOnInit() {
    //traigo contratos
    this.contratoServices.getContratosAutocomplete().subscribe(respuesta=>{
      this.contratos=respuesta;
      this.options=respuesta.map(function(task,index,array){return task.numeroContrato})
    });
    this.filteredOptions = this.numeroContrato.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );
    
  }

  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().indexOf(filterValue) === 0);
  }

  public seleccionAutocomplete(numeroContrato)
  {
    this.numeroContratoSeleccionado=numeroContrato;
  }

  public changeNovedadAplicada()
  {
    console.log(this.novedadAplicada.value);
    if(!this.novedadAplicada)
    {
      this.contratoServices.getProyectosContrato(this.numeroContratoSeleccionado).subscribe(
        response=>
        {
          this.proyectos=response;
        }
      );
    }
    this.contrato=this.contratos.filter(x=>x.numeroContrato==this.numeroContratoSeleccionado)[0];
    console.log(this.contrato);
  }

}
