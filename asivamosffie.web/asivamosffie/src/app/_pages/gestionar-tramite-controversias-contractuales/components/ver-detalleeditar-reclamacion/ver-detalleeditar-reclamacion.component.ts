import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalleeditar-reclamacion',
  templateUrl: './ver-detalleeditar-reclamacion.component.html',
  styleUrls: ['./ver-detalleeditar-reclamacion.component.scss']
})
export class VerDetalleeditarReclamacionComponent implements OnInit {
  idControversia: any;
  idActuacion : any;
  numReclamacion: any;
  actuacion: any;
  numActuacion: any;
  reclamacion: any;
  constructor(private activatedRoute: ActivatedRoute, private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.idControversia = param.idControversia;
      this.idActuacion = param.id;
      this.loadObservacionComite(this.idActuacion);
    });
  }
  loadObservacionComite(id){
    this.services.GetControversiaActuacionById(id).subscribe((data:any)=>{
      this.reclamacion = data;
    });
  }
}
