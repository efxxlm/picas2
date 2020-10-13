import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActBeginService } from 'src/app/core/_services/actBegin/act-begin.service';

@Component({
  selector: 'app-ver-detalle-tecnico-fdos-constr',
  templateUrl: './ver-detalle-tecnico-fdos-constr.component.html',
  styleUrls: ['./ver-detalle-tecnico-fdos-constr.component.scss']
})
export class VerDetalleTecnicoFdosConstrComponent implements OnInit {
  public rolAsignado;
  public opcion;
  public idContrato;
  constructor(private activatedRoute: ActivatedRoute,private services: ActBeginService) { }

  ngOnInit(): void {
    this.cargarRol();
    this.activatedRoute.params.subscribe(param => {
      this.loadData(param.id);
    });
  }

  cargarRol() {
    this.rolAsignado = JSON.parse(localStorage.getItem("actualUser")).rol[0].perfilId;
    if (this.rolAsignado == 2) {
      this.opcion = 1;
    }
    else {
      this.opcion = 2;
    }
  }

  loadData(id){
    this.idContrato = id;
    alert(this.idContrato);
  }
  descargarActaSuscrita(){
    this.services.GetPlantillaActaInicio(this.idContrato).subscribe(resp=>{
      const documento = `Prueba.pdf`; // Valor de prueba
      const text = documento,
      blob = new Blob([resp], { type: 'application/pdf' }),
      anchor = document.createElement('a');
      anchor.download = documento;
      anchor.href = window.URL.createObjectURL(blob);
      anchor.dataset.downloadurl = ['application/pdf', anchor.download, anchor.href].join(':');
      anchor.click();
    });
  }
}
