import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProcesoSeleccion, ProcesoSeleccionService } from 'src/app/core/_services/procesoSeleccion/proceso-seleccion.service';

@Component({
  selector: 'app-monitorear-cronograma',
  templateUrl: './monitorear-cronograma.component.html',
  styleUrls: ['./monitorear-cronograma.component.scss']
})
export class MonitorearCronogramaComponent implements OnInit {

  verAyuda = false;

  ajustarCronograma = false;
  registrarSeguimiento = false;
  editMode = { valor: true };
  procesoSeleccion: ProcesoSeleccion = {
    alcanceParticular: '',
    criteriosSeleccion: '',
    esDistribucionGrupos:false,
    justificacion: '',
    numeroProceso: '',
    objeto: '',
    procesoSeleccionId: 0,
    tipoAlcanceCodigo: '',
    tipoIntervencionCodigo: '',
    tipoProcesoCodigo: '',
    procesoSeleccionGrupo: [],
    procesoSeleccionCronograma: [],

  };


  constructor(private activatedRoute: ActivatedRoute,private procesoSeleccionService: ProcesoSeleccionService,) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe( async parametro => {
      //this.procesoSeleccion.tipoProcesoCodigo = parametro['tipoProceso'];
      this.procesoSeleccion.procesoSeleccionId = parametro['id'];      
      if (this.procesoSeleccion.procesoSeleccionId > 0)
      {
        this.procesoSeleccionService.getProcesoSeleccionById( this.procesoSeleccion.procesoSeleccionId ).subscribe(
          retorno=>{
            this.procesoSeleccion=retorno;
          }
        );
        
      }
    })
  }

}
