import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ContractualNoveltyService } from 'src/app/core/_services/ContractualNovelty/contractual-novelty.service';
import { NovedadContractual } from 'src/app/_interfaces/novedadContractual';

@Component({
  selector: 'app-registrar-solicitud',
  templateUrl: './registrar-solicitud.component.html',
  styleUrls: ['./registrar-solicitud.component.scss']
})
export class RegistrarSolicitudComponent implements OnInit {

  numeroContrato = new FormControl();
  novedadAplicada = new FormControl();
  filteredOptions: Observable<string[]>;
  //options: string[] = [];
  options: any[] = [];

  novedadesArray = [
    /*{ name: 'Contrato', value: true },*/
    { name: 'Proyecto', value: false }
  ];

  novedadesArrayProy = [
    { name: 'Proyecto', value: false }
  ];
  estaEditando = false;

  constructor(
    private contractualNoveltyService: ContractualNoveltyService,
    private activatedRoute: ActivatedRoute,

  ) {

  }
  numeroContratoSeleccionado: any;
  proyectos = [];
  proyecto: any;
  contratos = [];
  contrato: any;
  novedad: NovedadContractual = {};

  ngOnInit() {

    this.activatedRoute.params.subscribe(parametros => {

      //traigo contratos
      this.contractualNoveltyService.getContratosAutocomplete().subscribe(respuesta => {

        this.contratos = respuesta.filter(c => c.contratacion.tipoSolicitudCodigo === '1'); // obra
        //this.options=respuesta.map(function(task,index,array){return task.numeroContrato})
        this.options = respuesta.filter(c => c.contratacion.tipoSolicitudCodigo === '1'); // obra

        this.contractualNoveltyService.getNovedadContractualById(parametros.id)
          .subscribe(novedad => {
            console.log(novedad);

            if (novedad.novedadContractualId !== 0) {
              this.novedad = novedad;
              this.numeroContrato.setValue(novedad.contrato.numeroContrato);
              this.numeroContratoSeleccionado = novedad.contrato;
              this.novedadAplicada.setValue(novedad.esAplicadaAcontrato);
              this.proyecto = novedad['proyectosSeleccionado'];
              this.contrato = novedad.contrato;

              if (this.novedadAplicada.value == false) {
                this.contractualNoveltyService.getProyectosContrato(this.contrato.contratoId).subscribe(
                  response => {
                    this.proyectos = response;
                    console.log(this.proyectos);

                  }
                );
              } else {
                this.proyecto = null;
              }

              if (this.contrato !== undefined) {
                this.contratos.push(this.contrato)
                this.options.push(this.contrato)
              }
              this.estaEditando = true;
              this.numeroContrato.markAllAsTouched();
              this.novedadAplicada.markAllAsTouched();
            }
          });
        console.log(this.options)
      });
    });

    this.filteredOptions = this.numeroContrato.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value))
    );

  }

  private _filter(value: string): string[] {
    console.log(typeof value)
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.numeroContrato.toLowerCase().indexOf(filterValue) === 0);
  }

  public seleccionAutocomplete(numeroContrato) {
    if(numeroContrato.esMultiProyecto == true){
      this.novedadesArray = this.novedadesArray.filter(r => r.name == "Proyecto");
    }else{
      if(!this.novedadesArray.find(r =>r.name == "Contrato")){
        this.novedadesArray.push({ name: 'Contrato', value: true });
      }
    }
    this.numeroContratoSeleccionado = numeroContrato;
    this.contrato = null;
    this.proyecto = null;
    this.novedadAplicada.setValue(null);
    //console.log(numeroContrato);
  }

  public changeNovedadAplicada() {
    console.log(this.novedadAplicada.value);
    this.novedad.esAplicadaAcontrato = this.novedadAplicada.value;

    if (this.novedadAplicada.value == false) {
      this.contractualNoveltyService.getProyectosContrato(this.numeroContratoSeleccionado.contratoId).subscribe(
        response => {
          this.proyectos = response;
          console.log(this.proyectos);

        }
      );
    } else {
      this.proyecto = null;
    }
    console.log(this.numeroContratoSeleccionado, this.contratos, this.contratos.filter(x => x.numeroContrato == this.numeroContratoSeleccionado.numeroContrato));
    this.contrato = this.contratos.filter(x => x.numeroContrato == this.numeroContratoSeleccionado.numeroContrato)[0];
    console.log(this.contrato, this.proyecto);
  }

  seleccionarProyecto(proy) {
    console.log(proy, this.contrato)
    this.proyecto = proy;
  }

}
